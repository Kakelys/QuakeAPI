using FluentValidation;

namespace QuakeAPI.DTO.Notifications.Validators
{
    public class TelegramNotificationValidator : AbstractValidator<TelegramNotification>
    {
        public TelegramNotificationValidator()
        {
            RuleFor(x => x.Message).NotEmpty();
        }
    }
}