using QuakeAPI.Data.Repository.Interfaces;

namespace QuakeAPI.Data.Repository
{
    public class RepositoryManager : IRepositoryManager
    {
        private readonly QuakeDbContext _context;

        private readonly IAccountRepository _account;
        private readonly ILocationRepository _location;
        private readonly ISessionRepository _session;
        private readonly IActiveAccountRepository _activeAccount;
        private readonly ITokenRepository _token;
        private readonly IAnalyticRepository _analytic;
        private readonly INotificationRepository _notification;

        public RepositoryManager(
            QuakeDbContext context,
            IAccountRepository account,
            ILocationRepository location,
            ISessionRepository session,
            IActiveAccountRepository activeAccount,
            ITokenRepository token,
            IAnalyticRepository analytic,
            INotificationRepository notification)
        {
            _context = context;
            _account = account;
            _location = location;
            _session = session;
            _activeAccount = activeAccount;
            _token = token;
            _analytic = analytic;
            _notification = notification;
        }

        public IAccountRepository Account => _account;
        public ILocationRepository Location => _location;
        public ISessionRepository Session => _session;
        public IActiveAccountRepository ActiveAccount => _activeAccount;
        public ITokenRepository Token => _token;
        public IAnalyticRepository Analytic => _analytic;
        public INotificationRepository Notification => _notification;

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