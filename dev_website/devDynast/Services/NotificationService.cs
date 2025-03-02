using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using devDynast.Services;
using devDynast.Data;
using devDynast.Models;

namespace devDynast.Services
{
    public class NotificationService
    {
        private readonly ApplicationDbContext _context;

        public NotificationService(ApplicationDbContext context)
        {
            _context = context;
        }

        public NotificationsViewModel GetNotifications(int? userId, bool isAdmin)
        {
            var notificationsQuery = _context.Notifications.AsQueryable();

            if (isAdmin)
            {
                // Fetch all unread notifications for admin
                notificationsQuery = notificationsQuery.Where(n => !n.IsRead);
            }
            else if (userId.HasValue)
            {
                // Fetch unread notifications for the specific user
                notificationsQuery = notificationsQuery.Where(n => n.UserId == userId && !n.IsRead);
            }

            var notifications = notificationsQuery.ToList();

            return new NotificationsViewModel
            {
                Notifications = notifications,
                UnreadNotificationCount = notifications.Count(n => !n.IsRead)
            };
        }
    }
}
