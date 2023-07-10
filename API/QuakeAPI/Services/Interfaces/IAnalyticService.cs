using QuakeAPI.Data.Models;

namespace QuakeAPI.Services.Interfaces
{
    public interface IAnalyticService
    {
        Location? MostPopularLocationByMonth(int month, int year);
    }
}