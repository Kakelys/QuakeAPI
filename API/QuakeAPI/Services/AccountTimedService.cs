using QuakeAPI.Services.Interfaces;

namespace QuakeAPI.Services
{
    public class AccountTimedService : IAccountTimedService
    {
        private readonly ILogger<AccountTimedService> _logger;
        private readonly IAccountService _accountService;

        public AccountTimedService(
            ILogger<AccountTimedService> logger,
            IAccountService accountService)
        {
            _logger = logger;
            _accountService = accountService;
        }

        public async Task DeleteOldAccounts(CancellationToken stoppingToken)
        {
            while(!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation($"Deleting old accounts; Current time(utc): {DateTime.UtcNow}");

                await _accountService.DeleteOldAccounts();
                
                //once per day
                await Task.Delay(1000*60*60*24, stoppingToken);
            }
        }
    }
}