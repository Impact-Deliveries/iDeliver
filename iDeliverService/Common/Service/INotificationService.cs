using IDeliverObjects.DTO.Notification;

namespace iDeliverService.Common.Service
{
    public interface INotificationService
    {
        Task<Response> SendNotification(Notification notification);
    }
}