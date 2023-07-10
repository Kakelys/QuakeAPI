namespace QuakeAPI.Services.Interfaces
{
    public interface IAccountTimedService
    {
        Task DeleteOldAccounts(CancellationToken stoppingToken);
    }
}