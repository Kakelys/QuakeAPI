using Microsoft.EntityFrameworkCore;
using QuakeAPI.Data.Models;
using QuakeAPI.Data.Repository.Interfaces;

namespace QuakeAPI.Data.Repository.Implements
{
    public class AnalyticRepository : IAnalyticRepository
    {
        private readonly QuakeDbContext _context;

        public AnalyticRepository(QuakeDbContext context)
        {
            _context = context;
        }

        public List<Account> FindAccountsActivierThanAvg()
        {
            return null;
        }

        public Location? FindMostPopularLocationByMonth(int month, int year)
        {
            var result = _context.Locations.FromSqlInterpolated(@$"
                        with location as (
                            select top 1
                                l.Id, Count(month(s.Created)) [counts]
                            from Locations l
                            join Sessions s
                                on l.Id = s.LocationId
                            group by l.Id, month(s.Created), year(s.Created)
                            having month(s.Created) = {month} and year(s.Created) = {year})
                        select 
                            l.*
                        from location loc
                        join Locations l
                            on loc.Id = l.Id")
                        .AsEnumerable()
                        .FirstOrDefault();

            return result;
        }
    }
}