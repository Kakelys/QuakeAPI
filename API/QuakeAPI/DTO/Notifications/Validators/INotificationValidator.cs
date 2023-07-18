using FluentValidation;

namespace QuakeAPI.DTO.Notifications.Validators
{
    public interface INotificationValidator
    {
        IValidator<EmailNotification> EmailValidator {get;}
        IValidator<TelegramNotification> TelegramValidator {get;}
        IValidator<SaveNotification> SaveValidator {get;}
    }
}