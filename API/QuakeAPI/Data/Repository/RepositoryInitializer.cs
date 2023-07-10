using Microsoft.EntityFrameworkCore;
using QuakeAPI.Data.Repository.Implements;
using QuakeAPI.Data.Repository.Interfaces;

namespace QuakeAPI.Data.Repository
{
    public static class RepositoryInitializer
    {
        public static IServiceCollection AddRepositoryService(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<QuakeDbContext>(options => {
                options.UseSqlServer(config.GetConnectionString("QuakeDb"));
            });

            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<ILocationRepository, LocationRepository>();
            services.AddScoped<IActiveAccountRepository, ActiveAccountRepository>();
            services.AddScoped<ISessionRepository, SessionRepository>();
            services.AddScoped<ITokenRepository, TokenRepository>();
            services.AddScoped<IAnalyticRepository, AnalyticRepository>();

            services.AddScoped<IRepositoryManager, RepositoryManager>();

            return services;
        } 
    }
}