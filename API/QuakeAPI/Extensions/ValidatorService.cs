using FluentValidation;
using FluentValidation.AspNetCore;
using QuakeAPI.DTO;
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

            return services;
        }
    }
}