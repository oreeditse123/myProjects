using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using devDynast.Data;
using devDynast.Models;

namespace devDynast.Controllers
{
    public class BaseController : Controller
    {
         protected readonly ApplicationDbContext _context;
         protected readonly ILogger<BaseController> _logger; 

    public BaseController(ApplicationDbContext context, ILogger<BaseController> logger)
    {
        _context = context;
        _logger = logger; 
    }

    public void LoadNotifications()
    {
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

        
        ViewBag.UnreadNotificationCount = notifications.Count;
        ViewBag.Notifications = notifications; 

        
        foreach (var notification in notifications)
        {
            _logger.LogInformation("Notification: Id={Id}, UserId={UserId}, Message={Message}, CreatedAt={CreatedAt}, IsRead={IsRead}, OrderId={OrderId}",
                notification.Id, notification.UserId, notification.Message, notification.CreatedAt, notification.IsRead, notification.OrderId);
        }
    }
    }
}
