using FluentValidation;

namespace QuakeAPI.DTO.Notifications.Validators
{
    public class NotificationValidator : INotificationValidator
    {
        private readonly IValidator<EmailNotification> _emailValidator;
        private readonly IValidator<TelegramNotification> _telegramValidator;
        private readonly IValidator<SaveNotification> _saveValidator;

        public NotificationValidator(
            IValidator<EmailNotification> emailValidator,
            IValidator<TelegramNotification> telegramValidator,
            IValidator<SaveNotification> saveValidator
        )
        {
            _emailValidator = emailValidator;
            _telegramValidator = telegramValidator;
            _saveValidator = saveValidator;
        }

        public IValidator<EmailNotification> EmailValidator => _emailValidator;

        public IValidator<TelegramNotification> TelegramValidator => _telegramValidator;

        public IValidator<SaveNotification> SaveValidator => _saveValidator;
    }
}