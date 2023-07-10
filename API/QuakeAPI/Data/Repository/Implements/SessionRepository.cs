using Microsoft.EntityFrameworkCore;
using QuakeAPI.Data.Models;
using QuakeAPI.Data.Repository.Interfaces;

namespace QuakeAPI.Data.Repository.Implements
{
    public class SessionRepository : RepositoryBase<Session>, ISessionRepository
    {
        public SessionRepository(QuakeDbContext context) : base(context)
        {
        }

        public IQueryable<Session> FindByAccountId(int accountId, bool asTracking)
        {
            return _context.ActiveAccounts
                .Where(p => p.AccountId == accountId)
                .Include(p => p.Session.ActiveAccounts).Select(p => p.Session);
        }

        public IQueryable<Session> FindWithAccountsAndLocation()
        {
            return _context.Sessions
                .Where(s => s.DeletedAt == null)
                .Include(p => p.ActiveAccounts.Where(aa => aa.DisconnectedAt == null))
                .Include(p => p.Location);
        }
    }
}