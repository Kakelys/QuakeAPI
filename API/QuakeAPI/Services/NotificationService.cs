using System.Globalization;
using QuakeAPI.Data.Models;
using QuakeAPI.Data.Repository.Interfaces;
using QuakeAPI.DTO.Notifications;
using QuakeAPI.Exceptions;
using QuakeAPI.Services.Interfaces;

namespace QuakeAPI.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IEmailService _emailService;
        private readonly ITelegramService _telegramService;
        private readonly IRepositoryManager _rep;
        private readonly ILogger<NotificationService> _logger;

        public NotificationService(
            IEmailService emailService, 
            ITelegramService telegramService, 
            IRepositoryManager rep,
            ILogger<NotificationService> logger)
        {
            _emailService = emailService;
            _telegramService = telegramService;
            _rep = rep;
            _logger = logger;
        }

        public async Task Send(NotificationType notification, int accountId, object obj)
        {
            if(obj == null)
                throw new BadRequestException("Invalid notification object");

            var notificationMethods = new Dictionary<NotificationType, Func<int, object, Task>>
            {
                { NotificationType.Email, OnEmailNotification },
                { NotificationType.Telegram, OnTelegramNotification },
                { NotificationType.Save, OnSaveNotification }
            };
            try 
            {
                await notificationMethods[notification](accountId, obj);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error while sending notification");
                throw;
            }
        }

        private async Task OnEmailNotification(int accountId, object obj)
        {
            if(obj is not EmailNotification notification)
                throw new BadRequestException("Invalid notification object");

            var account = _rep.Account
                .FindNotDeleted(false)
                .FirstOrDefault(x => x.Id == accountId) ?? throw new NotFoundException("Account not found");

            await _emailService.Send(account.Email, notification.subject, notification.message);

        }

        private async Task OnTelegramNotification(int accountId, object obj)
        {
            if(obj is not TelegramNotification notification)
                throw new BadRequestException("Invalid notification object");

            var account = _rep.Account
                .FindNotDeleted(false)
                .FirstOrDefault(x => x.Id == accountId) ?? throw new NotFoundException("Account not found");
            if(account.TelegramChatId == 0)
                throw new BadRequestException("Accout is not linked with telegram");

            await _telegramService.SendMessage(account.TelegramChatId, notification.Message);
        }

        private async Task OnSaveNotification(int accountId, object obj)
        {
            if(obj is not SaveNotification notification)
                throw new BadRequestException("Invalid notification object");

            var account = _rep.Account
                .FindNotDeleted(false)
                .FirstOrDefault(x => x.Id == accountId) ?? throw new NotFoundException("Account not found");

            _rep.Notification.Create(new Notification
            {
                AccountId = accountId,
                Name = notification.NotificatioName,
                Data = notification.Data,
                CreatedAt = DateTime.UtcNow
            });

            await _rep.Save();
        }
    }
}