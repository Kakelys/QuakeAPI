using QuakeAPI.Data.Models;

namespace QuakeAPI.Services.Interfaces
{
    public interface INotificationService
    {
        Task Send(NotificationType notification, int accountId, string message, string? subject = null);
    }
}