using QuakeAPI.Data.Repository.Interfaces;

namespace QuakeAPI.Data.Repository
{
    public class RepositoryManager : IRepositoryManager
    {
        private readonly QuakeDbContext _context;

        private IAccountRepository _account;
        private ILocationRepository _location;
        private ISessionRepository _session;
        private IActiveAccountRepository _activeAccount;
        private ITokenRepository _token;
        private IAnalyticRepository _analytic;

        public RepositoryManager(
            QuakeDbContext context,
            IAccountRepository account,
            ILocationRepository location,
            ISessionRepository session,
            IActiveAccountRepository activeAccount,
            ITokenRepository token,
            IAnalyticRepository analytic)
        {
            _context = context;
            _account = account;
            _location = location;
            _session = session;
            _activeAccount = activeAccount;
            _token = token;
            _analytic = analytic;
        }

        public IAccountRepository Account => _account;

        public ILocationRepository Location => _location;

        public ISessionRepository Session => _session;

        public IActiveAccountRepository ActiveAccount => _activeAccount;

        public ITokenRepository Token => _token;

        public IAnalyticRepository Analytic => _analytic;

        public async Task BeginTransaction()
        {
            await _context.Database.BeginTransactionAsync();
        }

        public async Task Commit()
        {
            await _context.Database.CommitTransactionAsync();
        }

        public async Task Rollback()
        {
            await _context.Database.RollbackTransactionAsync();
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }
    }
}