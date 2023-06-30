using Microsoft.EntityFrameworkCore;
using QuakeAPI.Data.Models;
using QuakeAPI.Data.Repository.Interfaces;

namespace QuakeAPI.Data.Repository.Implements
{
    public class TokenRepository : RepositoryBase<Token>, ITokenRepository
    {
        public TokenRepository(QuakeDbContext context) : base(context)
        {
        }

        public IQueryable<Token> FindByToken(string refreshToken, bool asTracking)
        {
            return FindByCondition(t => t.RefreshToken == refreshToken, asTracking);
        }

        public IQueryable<Token> FindByTokenWithAccount(string refreshToken, bool asTracking)
        {
            return FindByCondition(t => t.RefreshToken == refreshToken, asTracking)
                .Include(t => t.Account);
        }
    }
}