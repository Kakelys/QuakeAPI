using QuakeAPI.Data.Models;
using QuakeAPI.DTO;

namespace QuakeAPI.Services.Interfaces
{
    public interface ITokenService
    {
        JwtPair CreatePair(Account account);
        string CreateToken(Account account, DateTime expires, string secret);
        Task<JwtPair> RefreshToken(string refreshToken);
        Task Revoke(string refreshToken);
    }
}