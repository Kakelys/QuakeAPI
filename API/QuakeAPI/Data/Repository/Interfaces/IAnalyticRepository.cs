using QuakeAPI.Data.Models;

namespace QuakeAPI.Data.Repository.Interfaces
{
    public interface IAnalyticRepository
    {
        Location? FindMostPopularLocationByMonth(int month, int year);
        List<Account> FindAccountsActivierThanAvg();
    }
}