namespace QuakeAPI.Data.Repository.Interfaces
{
    public interface IRepositoryManager
    {
        IAccountRepository Account { get; }
        ILocationRepository Location { get; }
        ISessionRepository Session { get; }
        IActiveAccountRepository ActiveAccount { get; }

        Task BeginTransaction();
        Task Commit();
        Task Rollback();
        Task Save();
    }
}