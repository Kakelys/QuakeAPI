using QuakeAPI.Data.Models;

namespace QuakeAPI.Data.Repository.Interfaces
{
    public interface ITokenRepository : IRepositoryBase<Token>
    {
        IQueryable<Token> FindByTokenWithAccount(string refreshToken, bool asTracking);
        IQueryable<Token> FindByToken(string refreshToken, bool asTracking);
    }
}