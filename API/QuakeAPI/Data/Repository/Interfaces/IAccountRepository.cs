using QuakeAPI.Data.Models;

namespace QuakeAPI.Data.Repository.Interfaces
{
    public interface IAccountRepository : IRepositoryBase<Account>
    {
        IQueryable<Account> FindByEmail(string email, bool asTracking);
        IQueryable<Account> FindByEmailWithTokens(string email, bool asTracking);
        IQueryable<Account> FindById(int id, bool asTracking);
    }
}