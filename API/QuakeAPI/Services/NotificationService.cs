using QuakeAPI.Data.Models;
using QuakeAPI.Services.Interfaces;

namespace QuakeAPI.Services
{
    public class NotificationService : INotificationService
    {
        public Task Send(NotificationType notification, int accountId, string message, string? subject = null)
        {
            throw new NotImplementedException();
        }
    }
}