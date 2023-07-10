using QuakeAPI.Data.Models;
using QuakeAPI.Data.Repository.Interfaces;
using QuakeAPI.Services.Interfaces;

namespace QuakeAPI.Services
{
    public class AnalyticService : IAnalyticService
    {   
        private IRepositoryManager _rep;

        public AnalyticService(IRepositoryManager rep)
        {
            _rep = rep;
        }

        public Location? MostPopularLocationByMonth(int month, int year)
        {
            return _rep.Analytic.FindMostPopularLocationByMonth(month, year);
        }
    }
}