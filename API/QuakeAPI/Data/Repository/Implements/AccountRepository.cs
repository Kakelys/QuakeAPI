using Microsoft.EntityFrameworkCore;
using QuakeAPI.Data.Models;
using QuakeAPI.Data.Repository.Interfaces;

namespace QuakeAPI.Data.Repository.Implements
{
    public class AccountRepository : RepositoryBase<Account>, IAccountRepository
    {
        public AccountRepository(QuakeDbContext context) : base(context)
        {
        }

        public IQueryable<Account> FindByEmail(string email, bool asTracking)
        {
            return FindByCondition(a => a.Email == email, asTracking);
        }

        public IQueryable<Account> FindByEmailWithTokens(string email, bool asTracking)
        {
            return FindByCondition(a => a.Email == email, asTracking)
                .Include(a => a.Tokens);
        }

        public IQueryable<Account> FindById(int id, bool asTracking)
        {
            return FindByCondition(a => a.Id == id, asTracking);
        }
    }
}