using QuakeAPI.DTO.Notifications;

namespace QuakeAPI.Services.Interfaces
{
    public interface INotificationService
    {
        Task Send(NotificationType notification, int accountId, object obj);
    }
}