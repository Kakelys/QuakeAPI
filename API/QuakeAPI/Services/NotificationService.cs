using Microsoft.EntityFrameworkCore;
using QuakeAPI.Data.Models;
using QuakeAPI.Data.Repository.Interfaces;
using QuakeAPI.DTO;
using QuakeAPI.DTO.Notifications;
using QuakeAPI.DTO.Notifications.Validators;
using QuakeAPI.Exceptions;
using QuakeAPI.Extensions;
using QuakeAPI.Services.Interfaces;

namespace QuakeAPI.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IEmailService _emailService;
        private readonly ITelegramService _telegramService;
        private readonly IRepositoryManager _rep;
        private readonly ILogger<NotificationService> _logger;
        private readonly INotificationValidator _validator;

        public NotificationService(
            IEmailService emailService, 
            ITelegramService telegramService, 
            IRepositoryManager rep,
            ILogger<NotificationService> logger,
            INotificationValidator validator)
        {
            _emailService = emailService;
            _telegramService = telegramService;
            _rep = rep;
            _logger = logger;
            _validator = validator;
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
                _logger.LogWarning(ex, "Error while sending notification");
                throw;
            }
        }

        public async Task Read(int accountId, Guid notificationId)
        {
            var notification = await _rep.Notification
                .FindByCondition(n => n.AccountId == accountId && n.Id == notificationId, true)
                .FirstOrDefaultAsync() ?? throw new NotFoundException("Notification not found");

            notification.ReadAt = DateTime.UtcNow;
            await _rep.Save();
        }

        public async Task<List<Notification>> GetPage(int accountId, string name, Page page)
        {
            return await _rep.Notification
                .FindByCondition(n => n.AccountId == accountId && n.Name == name && n.ReadAt == null, false)
                .TakePage(page)
                .ToListAsync();
        }

        private async Task OnEmailNotification(int accountId, object obj)
        {
            if(obj is not EmailNotification notification || !(await _validator.EmailValidator.ValidateAsync(notification)).IsValid)
                throw new BadRequestException("Invalid notification object");

            var account = _rep.Account
                .FindNotDeleted(false)
                .FirstOrDefault(x => x.Id == accountId) 
                ?? throw new NotFoundException("Account not found");

            await _emailService.Send(account.Email, notification.Subject, notification.Message);
        }

        private async Task OnTelegramNotification(int accountId, object obj)
        {
            if(obj is not TelegramNotification notification || !(await _validator.TelegramValidator.ValidateAsync(notification)).IsValid)
                throw new BadRequestException("Invalid notification object");

            var account = _rep.Account
                .FindNotDeleted(false)
                .FirstOrDefault(x => x.Id == accountId) 
                ?? throw new NotFoundException("Account not found");

            if(account.TelegramChatId == 0)
                throw new BadRequestException("Accout is not linked with telegram");

            await _telegramService.SendMessage(account.TelegramChatId, notification.Message);
        }

        private async Task OnSaveNotification(int accountId, object obj)
        {
            if(obj is not SaveNotification notification || !(await _validator.SaveValidator.ValidateAsync(notification)).IsValid)
                throw new BadRequestException("Invalid notification object");

            var account = _rep.Account
                .FindNotDeleted(false)
                .FirstOrDefault(x => x.Id == accountId) 
                ?? throw new NotFoundException("Account not found");

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