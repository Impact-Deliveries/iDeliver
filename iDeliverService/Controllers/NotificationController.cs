using IDeliverObjects.DTO.Notification;
using iDeliverService.Common.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace iDeliverService.Controllers
{
    [Route("api/notification")]
    [ApiController]
    [Authorize]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService<string> _notificationService;
        public NotificationController(INotificationService<string> notificationService)
        {
            _notificationService = notificationService;
        }

        [Route("send")]
        [HttpPost]
        public async Task<IActionResult> SendNotification(Notification<string> notification)
        {
            var result = await _notificationService.SendNotification(notification);
            return Ok(result);
        }
    }
}
