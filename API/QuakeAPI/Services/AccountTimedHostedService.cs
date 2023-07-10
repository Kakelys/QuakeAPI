using QuakeAPI.Services.Interfaces;

namespace QuakeAPI.Services
{
    public class AccountTimedHostedService : BackgroundService
    {
        public IServiceProvider Services { get; }

        private readonly ILogger<AccountTimedHostedService> _logger;

        public AccountTimedHostedService(
            ILogger<AccountTimedHostedService> logger,
            IServiceProvider services)
        {
            _logger = logger;
            Services = services;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await DoWork(stoppingToken);
        }

        private async Task DoWork(CancellationToken stoppingToken)
        {
            using var scope = Services.CreateScope();
            var scopedService =
                scope.ServiceProvider
                    .GetRequiredService<IAccountTimedService>();

            await scopedService.DeleteOldAccounts(stoppingToken);
        }

        public override async Task StopAsync(CancellationToken stoppingToken)
        {
            await base.StopAsync(stoppingToken);
        }
    }
}