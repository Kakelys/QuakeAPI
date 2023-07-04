using QuakeAPI.Data.Models;

namespace QuakeAPI.Data.Repository.Interfaces
{
    public interface ISessionRepository : IRepositoryBase<Session>
    {
        IQueryable<Session> FindByAccountId(int accountId, bool asTracking);
        IQueryable<Session> FindWithAccountsAndLocation();
    }
}