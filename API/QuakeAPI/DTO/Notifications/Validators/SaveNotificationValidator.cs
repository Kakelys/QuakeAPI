using FluentValidation;

namespace QuakeAPI.DTO.Notifications.Validators
{
    public class SaveNotificationValidator : AbstractValidator<SaveNotification>
    {
        public SaveNotificationValidator()
        {
            RuleFor(x => x.NotificatioName).NotEmpty();
        }
    }
}