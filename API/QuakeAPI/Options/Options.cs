namespace QuakeAPI.Options
{
    public static class Options
    {
        public static IServiceCollection AddAppOptions(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions<JwtOptions>().Bind(configuration.GetSection(JwtOptions.Jwt))
                .ValidateDataAnnotations()
                .ValidateOnStart();

            return services;
        }
    }
}