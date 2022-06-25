using IDeliverObjects.DTO.Notification;

namespace iDeliverService.Common.Service
{
    public interface INotificationService<T> where T : class
    {
        Task<Response> SendNotification(Notification<T> notification);
    }
}