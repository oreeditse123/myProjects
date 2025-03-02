using Microsoft.AspNetCore.Mvc;
using devDynast.Data;
using devDynast.Models;
using Microsoft.Extensions.Logging; 
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using devDynast.ViewModels;

namespace devDynast.Controllers
{
    [Authorize(Roles = "User")]
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<UserController> _logger;

        public UserController(ApplicationDbContext context, ILogger<UserController> logger) 
        {
            _context = context;
            _logger = logger; 
        }
        // GET: /User/Register
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // POST: /User/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(User user)
        {
            if (ModelState.IsValid)
            {
                // Add user to database
                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Home");
            }
            return View(user);
        }

        // GET: User/Dashboard
    public IActionResult Dashboard()
    {
        return View(); 
    }

    public IActionResult Menu(string category)
    {
        // Fetch all menus
    var menus = _context.Menus.ToList();

    // Fetch MenuItems based on the selected category, if provided
    var menuItems = string.IsNullOrEmpty(category) 
        ? _context.MenuItems.ToList() // If no category is selected, fetch all items
        : _context.MenuItems.Where(m => m.Category == category).ToList();

    var viewModel = new MenuViewModel
    {
        Menus = menus,
        MenuItems = menuItems // Pass the filtered menu items based on the category
    };

   return View("~/Views/User/Menu.cshtml", viewModel);
    }

    // GET: User/EditProfile
public async Task<IActionResult> EditProfile()
{
    var userId = HttpContext.Session.GetInt32("UserId");

    if (userId == null)
    {
        return NotFound();
    }

    var user = await _context.Users.FindAsync(userId.Value);
    if (user == null)
    {
        return NotFound();
    }

    return View(user);
}

 // POST: User/EditProfile
[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> EditProfile([Bind("Id,FirstName,LastName,Email,PhoneNumber,Role")] User user)
{
    _logger.LogInformation("EditProfile POST action called.");
    _logger.LogInformation("User profile requested for UserId: {UserId}", user.Id);

    var userId = HttpContext.Session.GetInt32("UserId");
    if (userId == null)
    {
        _logger.LogWarning("UserId is null in session.");
        return NotFound();
    }

    user.Id = userId.Value;

    if (ModelState.IsValid)
    {
        try
        {
            var existingUser = await _context.Users.FindAsync(userId);
            if (existingUser != null)
            {
                existingUser.FirstName = user.FirstName;
                existingUser.LastName = user.LastName;
                existingUser.PhoneNumber = user.PhoneNumber;
               

                _context.Update(existingUser);
                await _context.SaveChangesAsync();

                _logger.LogInformation("User profile updated successfully for UserId: {UserId}", user.Id);
                return RedirectToAction("Dashboard", "User"); 
            }
            else
            {
                _logger.LogWarning("User not found for UserId: {UserId}", user.Id);
                return NotFound();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating user profile for UserId: {UserId}", user.Id);
            ModelState.AddModelError(string.Empty, "An error occurred while updating the profile. Please try again.");
        }
    }
    else
    {
        _logger.LogWarning("ModelState is invalid for UserId: {UserId}", user.Id);
        foreach (var modelStateKey in ModelState.Keys)
        {
            var modelStateVal = ModelState[modelStateKey];
            foreach (var error in modelStateVal.Errors)
            {
                _logger.LogError("ModelState Error: Key = {Key}, Error = {Error}", modelStateKey, error.ErrorMessage);
            }
        }
    }

    return View(user); // Return the view with validation errors
}

// GET: Feedback/Create
        public IActionResult Feedback()
        {
            _logger.LogInformation("Navigated to Feedback/Create page.");
            return View();
        }

        // GET: User/Index
        public IActionResult Index()
        {
            return View();
        }

[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> Feedback(Feedback feedback)
{
    // Retrieve the user ID from the session
    var userId = HttpContext.Session.GetInt32("UserId");
    

    if (userId == null)
    {
        _logger.LogWarning("User not found in session.");
        return RedirectToAction("Error", "Home");
    }

    // Convert the nullable int to string
    feedback.UserId = userId.Value.ToString();

    if (ModelState.IsValid)
    {
        _context.Feedbacks.Add(feedback);
        await _context.SaveChangesAsync();
        return RedirectToAction("Index", "Home");
    }

    // Log the errors in the model state
    var errors = ModelState.Values.SelectMany(v => v.Errors);
    foreach (var error in errors)
    {
        _logger.LogWarning("Model error: {Error}", error.ErrorMessage);
    }

    return View(feedback);
}

// GET: Ratings
        public IActionResult Items()
{
    // Retrieve the user ID from the session as int
    var userIdInt = HttpContext.Session.GetInt32("UserId");
    
    // Convert to string
    var userId = userIdInt?.ToString();

    // Log the retrieved UserId
    _logger.LogInformation("Retrieved UserId from session: {UserId}", userId);

    // Check if the userId is not null
    if (!string.IsNullOrEmpty(userId))
    {
        var cartItems = _context.Cart
            .Where(c => c.UserId == userId && c.Status == "paid")
            .Join(_context.MenuItems,
                  cart => cart.ProductId,
                  menuItem => menuItem.Id.ToString(),
                  (cart, menuItem) => new
                  {
                      cart.ProductId,
                      cart.Quantity,
                      cart.Price,
                      cart.Status,
                      cart.CreatedAt,
                      ProductName = menuItem.Name 
                  })
            .ToList();

        return View(cartItems);
    }
    else
    {
        return RedirectToAction("Login", "Account");
    }
}



        // GET: Ratings/Create
        [HttpGet] 
        public IActionResult Create(int itemId)
        {
            ViewBag.ItemId = itemId;
            return View();
        }

       [HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> Create(Rating rating)
{
    _logger.LogInformation("Create action started with Rating: {@Rating}", rating);

    var menuItemExists = await _context.MenuItems.AnyAsync(m => m.Id == rating.ItemId);

    if (!menuItemExists)
    {
        _logger.LogWarning("The specified item with ItemId {ItemId} does not exist.", rating.ItemId);
        return Json(new { success = false, message = "The specified item does not exist." });
    }

    if (ModelState.IsValid)
    {
        _logger.LogInformation("ModelState is valid. Saving the rating.");
        
        rating.CreatedAt = DateTime.UtcNow;

        _context.Ratings.Add(rating);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Rating saved successfully with Id: {RatingId}", rating.Id);

        return Json(new { success = true, message = "Rating submitted successfully!" });
    }

    _logger.LogWarning("ModelState is invalid. Errors: {@ModelState}", ModelState.Values.SelectMany(v => v.Errors));
    return Json(new { success = false, message = "There was an error with your submission." });
}



public async Task<IActionResult> OrderHistory()
{
    // Retrieve the user ID from the session
    var userIdInt = HttpContext.Session.GetInt32("UserId");

    _logger.LogInformation("Retrieved UserId from session: {UserId}", userIdInt);

    if (userIdInt.HasValue)
    {
        var userId = userIdInt.Value.ToString();

        // Fetch past orders and join with the MenuItem to get product names
        var pastOrders = _context.Cart
            .Where(c => c.UserId == userId && c.Status == "paid")
            .Join(
                _context.MenuItems,  // The MenuItem table
                cart => cart.ProductId,  // The productId in the Cart table
                menu => menu.Id.ToString(),  // The Id in the MenuItem table
                (cart, menu) => new
                {
                    cart.Id,
                    cart.Quantity,
                    cart.Price,
                    cart.Status,
                    cart.CreatedAt,
                    ProductName = menu.Name  // Get the product name from MenuItem
                }
            )
            .ToList();

        // Calculate total price
        var totalPrice = pastOrders.Sum(c => c.Price * c.Quantity);

        // Pass orders and total price to the view
        ViewBag.TotalPrice = totalPrice;
        return View(pastOrders);
    }
    else
    {
        return RedirectToAction("Login", "Account");
    }
}

        [HttpPost]
    public ActionResult addToCart(String productId, decimal price)
    {
        try
        {
            // Retrieve the user ID from the session as int
            var userIdInt = HttpContext.Session.GetInt32("UserId");

            // Convert to string
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




    }


   
}
