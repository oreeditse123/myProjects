using Microsoft.AspNetCore.Mvc;
using devDynast.Services;
using devDynast.Models;

namespace devDynast.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        private readonly NotificationService _notificationService;

        public NotificationsController(NotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpGet("GetNotifications")]
        public IActionResult GetNotifications(int? userId, bool isAdmin)
        {
            var notificationsViewModel = _notificationService.GetNotifications(userId, isAdmin);
            return Ok(notificationsViewModel);
        }
    }
}
