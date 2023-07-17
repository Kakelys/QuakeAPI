using System.Security.Claims;
using QuakeAPI.Data.Models;
using QuakeAPI.DTO;

namespace QuakeAPI.Services.Interfaces
{
    public interface ITokenService
    {
        JwtPair CreatePair(Account account);
        string CreateToken(List<Claim> claims, DateTime expires, string secret);
        Task<JwtPair> RefreshToken(string refreshToken);
        bool Validate(string token, string secret);
        ClaimsPrincipal? ValidateWithClaims(string token, string secret);
        Task Revoke(string refreshToken);
    }
}