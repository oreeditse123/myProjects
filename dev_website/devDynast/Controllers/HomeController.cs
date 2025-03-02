using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using devDynast.Models;
using devDynast.Data;
using Microsoft.AspNetCore.Authorization;
using devDynast.Controllers;

namespace devDynast.Controllers;

[Authorize]
public class HomeController : BaseController
{

    private readonly ApplicationDbContext _context;

    private readonly ILogger<HomeController> _logger; // Add logger

        public HomeController(ApplicationDbContext context, ILogger<HomeController> logger)
            : base(context, logger) // Pass the logger to the base constructor
        {
            _context = context;
            _logger = logger; // Initialize the logger
        }
    public IActionResult Index()
    {
         LoadNotifications();
        return View();
    }

    public IActionResult Privacy()
    {
    
        return View();
    }

    public async Task<IActionResult> MenuTabs()
{
    
    var menus =  _context.Menus.ToList();

    if (menus == null || !menus.Any())
    {
       
        menus = new List<Menu>();
    }
    else
    {
       
    }

    return View(menus);
}


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    // Redirect to the menu page
    public IActionResult Menu()
    {
        return View("~/Views/User/Menu.cshtml"); 
    }

    // Redirect to the dashboard page
    public IActionResult Dashboard()
    {
        return RedirectToAction("Dashboard", "User");
    }

    private void LoadNotifications()
        {
            _logger.LogInformation("Loading notifications for user ID: ");
            var userIdString = HttpContext.Session.GetString("UserId");
            var role = HttpContext.Session.GetString("Role");

            List<NotificationViewModel> notifications;

            _logger.LogInformation("Loading notifications for user ID: {UserId} and role: {Role}", userIdString, role);

            if (role == "Admin")
            {
                // Fetch all unread notifications for admin
                notifications = _context.Notifications
                    .Where(n => !n.IsRead)
                    .Select(n => new NotificationViewModel
                    {
                        Id = n.Id,
                        UserId = n.UserId,
                        Message = n.Message,
                        CreatedAt = n.CreatedAt,
                        IsRead = n.IsRead,
                        OrderId = n.OrderId
                    })
                    .ToList();

                _logger.LogInformation("Fetched {Count} unread notifications for admin.", notifications.Count);
            }
            else if (int.TryParse(userIdString, out int userId))
            {
                // Fetch unread notifications for the specific user
                notifications = _context.Notifications
                    .Where(n => n.UserId == userId && !n.IsRead)
                    .Select(n => new NotificationViewModel
                    {
                        Id = n.Id,
                        UserId = n.UserId,
                        Message = n.Message,
                        CreatedAt = n.CreatedAt,
                        IsRead = n.IsRead,
                        OrderId = n.OrderId
                    })
                    .ToList();

                _logger.LogInformation("Fetched {Count} unread notifications for user ID: {UserId}.", notifications.Count, userId);
            }
            else
            {
                // No user logged in
                notifications = new List<NotificationViewModel>();
                _logger.LogWarning("No user logged in. Notifications list is empty.");
            }

            // Populate the ViewBag with notifications and unread count
            ViewBag.UnreadNotificationCount = notifications.Count;
            ViewBag.Notifications = notifications; // This will hold the list of notifications

            // Optionally log the fetched notifications for further inspection
            foreach (var notification in notifications)
            {
                _logger.LogInformation("Notification: Id={Id}, UserId={UserId}, Message={Message}, CreatedAt={CreatedAt}, IsRead={IsRead}, OrderId={OrderId}",
                    notification.Id, notification.UserId, notification.Message, notification.CreatedAt, notification.IsRead, notification.OrderId);
            }
        }
}


