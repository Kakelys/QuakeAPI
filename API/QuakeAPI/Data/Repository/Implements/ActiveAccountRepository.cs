using QuakeAPI.Data.Models;
using QuakeAPI.Data.Repository.Interfaces;

namespace QuakeAPI.Data.Repository.Implements
{
    public class ActiveAccountRepository : RepositoryBase<ActiveAccount>, IActiveAccountRepository
    {
        public ActiveAccountRepository(QuakeDbContext context) : base(context)
        {
        }
    }
}