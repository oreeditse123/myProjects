using System;
using devDynast.Models;
using devDynast.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using devDynast.ViewModels;
using Microsoft.Extensions.Logging; 
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO; 

public class CartController : Controller
{
     private readonly ApplicationDbContext _context;
    private readonly ILogger<CartController> _logger; 

    public CartController(ApplicationDbContext context, ILogger<CartController> logger) 
    {
        _context = context;
        _logger = logger; 
    }

    [HttpPost]
    public ActionResult AddToCart(String productId, decimal price)
    {
        try
        {
            // Retrieve the user ID from the session as int
            var userIdInt = HttpContext.Session.GetInt32("UserId");

            
            var userId = userIdInt?.ToString();

            var cartItem = new Cart
            {
                UserId = userId,
                ProductId = productId,
                Quantity = 1,  
                Price = (double)price,
                Status = "pending",
                CreatedAt = DateTime.UtcNow
            };

            _context.Cart.Add(cartItem);
            _context.SaveChanges();

            return Json(new { success = true, message = "Item added to cart." });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = "Error adding item to cart: " + ex.Message });
        }
    }

   public IActionResult Cart()
{
    
    var userIdInt = HttpContext.Session.GetInt32("UserId");
    var userId = userIdInt?.ToString();

    // Create an instance of MenuViewModel
    var model = new MenuViewModel();

    
    if (!string.IsNullOrEmpty(userId))
    {
         _logger.LogInformation($"User ID retrieved from session: {userId}");
        var cartItems = _context.Cart
            .Where(c => c.UserId == userId && c.Status == "pending")
            .Join(_context.MenuItems,
                  cart => cart.ProductId,
                  menuItem => menuItem.Id.ToString(),
                  (cart, menuItem) => new CartItemViewModel
                  {
                      Id = cart.Id, 
                      Quantity = cart.Quantity,
                      Price = cart.Price,
                      ProductName = menuItem.Name,
                      ProductImage = menuItem.ImageUrl,
                      Category = menuItem.Category
                  })
            .ToList();

            _logger.LogInformation($"Number of items found in cart: {cartItems.Count}");

            if (cartItems.Count > 0)
            {
                model.CartItems = cartItems; 
                _logger.LogInformation("Cart items successfully added to model.");
            }
            else
            {
                _logger.LogWarning("No items found in the cart for the user.");
                
            }

       

        return View("~/Views/User/Cart.cshtml", model); 

        
    }
    else
    {
          _logger.LogWarning("User ID is null. Redirecting to login.");
        return RedirectToAction("Login", "Account");
    }
}

[HttpPost]
public IActionResult RemoveFromCart(int id)
{
    // Retrieve the user ID from the session
    var userIdInt = HttpContext.Session.GetInt32("UserId");
    var userId = userIdInt?.ToString();


    if (!string.IsNullOrEmpty(userId))
    {
        var cartItem = _context.Cart.FirstOrDefault(c => c.Id == id && c.UserId == userId);
        
        if (cartItem != null)
        {
            _context.Cart.Remove(cartItem); 
            _context.SaveChanges(); // Save changes to the database
            return Json(new { success = true });
        }
        else
        {
            return Json(new { success = false, message = "Item not found." });
        }
    }
    
    return Json(new { success = false, message = "User ID is null." });
}

[HttpPost]
public IActionResult UpdateQuantity(int id, int quantity)
{
    
    var userIdInt = HttpContext.Session.GetInt32("UserId");
    var userId = userIdInt?.ToString();

    // Check if the userId is not null
    if (!string.IsNullOrEmpty(userId))
    {
        var cartItem = _context.Cart.FirstOrDefault(c => c.Id == id && c.UserId == userId);
        
        if (cartItem != null)
        {
            cartItem.Quantity = quantity; // Update the quantity
            _context.SaveChanges();
            return Json(new { success = true });
        }
        else
        {
            return Json(new { success = false, message = "Item not found." });
        }
    }
    
    return Json(new { success = false, message = "User ID is null." });
}

public IActionResult Checkout()
{
    
    var userIdInt = HttpContext.Session.GetInt32("UserId");
    var userId = userIdInt?.ToString();

    if (!string.IsNullOrEmpty(userId))
    {
        _logger.LogInformation($"User ID retrieved from session: {userId}");

        // Retrieve cart items and join with menu items to get product details
        var cartItems = _context.Cart
            .Where(c => c.UserId == userId && c.Status == "pending")
            .Join(_context.MenuItems,
                  cart => cart.ProductId,
                  menuItem => menuItem.Id.ToString(),
                  (cart, menuItem) => new CartItemViewModel
                  {
                      Id = cart.Id, 
                      Quantity = cart.Quantity,
                      Price = cart.Price,
                      ProductName = menuItem.Name,
                      ProductImage = menuItem.ImageUrl,
                      Category = menuItem.Category
                  })
            .ToList();

        if (cartItems.Any())
        {
            // Calculate total price and other details for the notification
            var totalItems = cartItems.Count;
            var totalPrice = cartItems.Sum(c => c.Quantity * c.Price);

            // Update the status of each item to 'paid'
            foreach (var item in cartItems)
            {
                var cartItem = _context.Cart.Find(item.Id);
                cartItem.Status = "paid";
            }

            _context.SaveChanges(); // Save the changes to the database

            // Find all users with the Admin role
            var adminUsers = _context.Users
                .Where(u => u.Role == "Admin") 
                .ToList();

            
            foreach (var admin in adminUsers)
            {
                var notificationMessage = $"A new order has been placed with {totalItems} items totaling R{totalPrice}.";
                var notification = new NotificationViewModel
                {
                    Message = notificationMessage, 
                    UserId = (int)admin.Id, 
                    CreatedAt = DateTime.UtcNow,
                };

                _context.Notifications.Add(notification);
            }

            _context.SaveChanges(); // Save the notifications

            // Generate the receipt
            var receiptPath = GenerateReceiptPdf(userId, cartItems, totalPrice);
        
            // Return success with receipt path
            return Json(new { success = true, message = "Checkout successful!", receiptUrl = Url.Action("ViewReceipt", new { receiptPath = receiptPath }) });
        }
        else
        {
            return Json(new { success = false, message = "No items in the cart." });
        }
    }
    else
    {
        return Json(new { success = false, message = "User not logged in." });
    }
}




private string GenerateReceiptPdf(string userId, List<CartItemViewModel> cartItems, double totalPrice)
{
    var receiptPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "receipts", $"receipt_{userId}_{DateTime.UtcNow.Ticks}.pdf");

    var (firstName, lastName) = GetUserDetails(userId);

    using (FileStream fs = new FileStream(receiptPath, FileMode.Create))
    using (var doc = new iTextSharp.text.Document())
    {
        var writer = iTextSharp.text.pdf.PdfWriter.GetInstance(doc, fs);
        doc.Open();

        // Add receipt details
        doc.Add(new iTextSharp.text.Paragraph("Receipt"));
        doc.Add(new iTextSharp.text.Paragraph($"Date: {DateTime.UtcNow}"));
        doc.Add(new iTextSharp.text.Paragraph($"User ID: {userId}"));
         doc.Add(new iTextSharp.text.Paragraph($"Name: {firstName} {lastName}"));
        doc.Add(new iTextSharp.text.Paragraph("Items:"));

        // Add each cart item to the PDF
        foreach (var item in cartItems)
        {
            doc.Add(new iTextSharp.text.Paragraph($"{item.ProductName} - Quantity: {item.Quantity}, Price: R{item.Price}"));
        }

        // Add total price
        doc.Add(new iTextSharp.text.Paragraph($"Total Price: R{totalPrice}"));

        doc.Close();
    }

    return receiptPath.Replace("\\", "/"); 
}

public User GetUserById(string userId)
    {
        
        if (int.TryParse(userId, out int id))
        {
            return _context.Users.FirstOrDefault(u => u.Id == id);
        }
        return null;
    }

    private (string FirstName, string LastName) GetUserDetails(string userId)
{
    var user = GetUserById(userId); 
    return (user?.FirstName ?? "N/A", user?.LastName ?? "N/A");
}

public IActionResult ViewReceipt(string receiptPath)
{
    if (!string.IsNullOrEmpty(receiptPath))
    {
        var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", receiptPath);

        if (System.IO.File.Exists(fullPath))
        {
            // Return the file for the user to view or download
            var fileBytes = System.IO.File.ReadAllBytes(fullPath);
            return File(fileBytes, "application/pdf", "Receipt.pdf");
        }
    }

    return NotFound("Receipt not found.");
}


[HttpPost]
public IActionResult MarkNotificationsAsRead()
{
    
    var notifications = _context.Notifications
        .Where(n => !n.IsRead)
        .ToList();

    foreach (var notification in notifications)
    {
        notification.IsRead = true;
        notification.OrderId = "1";
    }

    _context.SaveChanges();

    return Json(new { success = true });
}






public IActionResult GetOrderDetails()
{
    // Fetch the pending orders along with product and user information
    var cartItems = _context.Cart
        .Where(c => c.Status == "paid")
        .Join(_context.MenuItems,
              cart => cart.ProductId,
              menuItem => menuItem.Id.ToString(),
              (cart, menuItem) => new
              {
                  cart.Id,
                  cart.Quantity,
                  cart.Price,
                  cart.UserId,
                  ProductName = menuItem.Name,
                  ProductImage = menuItem.ImageUrl,
                  Category = menuItem.Category,
                  Date = cart.CreatedAt,
                  cart.Status
              })
        .Join(_context.Users,
              cartItem => cartItem.UserId,
              user => user.Id.ToString(),
              (cartItem, user) => new OrderDetailViewModel
              {
                  Id = cartItem.Id,
                  ProductName = cartItem.ProductName,
                  Price = cartItem.Price,
                  Quantity = cartItem.Quantity,
                  Date = cartItem.Date,
                  ProductImage = cartItem.ProductImage,
                  Category = cartItem.Category,
                  FirstName = user.FirstName, 
                  LastName = user.LastName,
                  Status = cartItem.Status
              })
        .ToList();

    // Return the order details to the view or as JSON
     return View("~/Views/Admin/OrderDetails.cshtml", cartItems);  
}

[HttpPost]
public IActionResult UpdateOrderStatus(int cartId, string status)
{
    _logger.LogInformation("UpdateOrderStatus called with cartId: {CartId}, new status: {Status}", cartId, status);

    var cartItem = _context.Cart.Find(cartId);
    if (cartItem != null)
    {
        cartItem.Status = status; // Update the status
        _context.SaveChanges(); // Save the changes to the database
        
        _logger.LogInformation("Cart item with Id: {CartId} status updated to {Status}", cartId, status);
    }
    else
    {
        _logger.LogWarning("Cart item with Id: {CartId} not found", cartId);
    }

    // Redirect back to the order details view after updating
    return RedirectToAction("GetOrderDetails");
}






}

