using QuakeAPI.Data.Models;
using QuakeAPI.DTO;
using QuakeAPI.DTO.Notifications;

namespace QuakeAPI.Services.Interfaces
{
    public interface INotificationService
    {
        Task Send(NotificationType notification, int accountId, object obj);
        Task Read(int accountId, Guid notificationId);
        Task<List<Notification>> GetPage(int accountId, string name, Page page);
    }
}