using FluentValidation;
using FluentValidation.AspNetCore;
using QuakeAPI.DTO;
using QuakeAPI.DTO.Notifications;
using QuakeAPI.DTO.Notifications.Validators;
using QuakeAPI.Validators;

namespace QuakeAPI.Extensions
{
    public static class ValidatorService
    {
        public static IServiceCollection AddValidatorService(this IServiceCollection services)
        {
            services.AddFluentValidationAutoValidation(fv => 
            {
                fv.DisableDataAnnotationsValidation = true;
            });
            services.AddScoped<IValidator<AccountUpdate>, AccountUpdateValidator>();
            services.AddScoped<IValidator<LocationNew>, LocationNewValidator>();

            //notification validators
            services.AddScoped<IValidator<EmailNotification>, EmailNotificationValidator>();
            services.AddScoped<IValidator<TelegramNotification>, TelegramNotificationValidator>();
            services.AddScoped<IValidator<SaveNotification>, SaveNotificationValidator>();
            services.AddScoped<INotificationValidator, NotificationValidator>();

            return services;
        }
    }
}