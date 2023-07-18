using FluentValidation;

namespace QuakeAPI.DTO.Notifications.Validators
{
    public class EmailNotificationValidator : AbstractValidator<EmailNotification>
    {
        public EmailNotificationValidator()
        {
            RuleFor(x => x.Subject).NotEmpty();
            RuleFor(x => x.Message).NotEmpty();
        }
        
    }
}