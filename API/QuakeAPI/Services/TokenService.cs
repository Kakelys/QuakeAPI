using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using QuakeAPI.Data.Models;
using QuakeAPI.Data.Repository.Interfaces;
using QuakeAPI.DTO;
using QuakeAPI.Services.Interfaces;

namespace QuakeAPI.Services
{
    public class TokenService : ITokenService
    {
        private IRepositoryManager _rep;
        private IConfiguration _config;

        public TokenService(IRepositoryManager rep, IConfiguration config)
        {
            _rep = rep;
            _config = config;
        }

        public JwtPair CreatePair(Account account)
        {
            var accessToken = CreateToken(account, NewAccessTokenExpirationDate(), _config["Jwt:AccessSecret"]);
            var refreshToken = CreateToken(account, NewRefreshTokenExpirationDate(), _config["Jwt:RefreshSecret"]);

            return new JwtPair
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }

        public string CreateToken(Account account, DateTime expires, string? secret)
        {
            var issuer = _config["Jwt:Issuer"];
            var audience = _config["Jwt:Audience"];
            if(string.IsNullOrEmpty(secret))
                throw new ArgumentNullException("Jwt:AccessSecret or Jwt:RefreshSecret");
            var key = Encoding.ASCII.GetBytes(secret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, account.Id.ToString()),
                    new Claim(ClaimTypes.Role, account.Role),
                }),
                Expires = expires,
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials
                    (new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha512Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = tokenHandler.WriteToken(token);

            return jwtToken;
        }

        public DateTime NewAccessTokenExpirationDate()
        {
            var accessLifetime = _config["Jwt:AccessLifetimeInMinutes"];
            if(string.IsNullOrEmpty(accessLifetime))
                throw new ArgumentNullException("Jwt:AccessLifetimeInMinutes");

            return DateTime.UtcNow.AddMinutes(Int32.Parse(accessLifetime));
        }

        public DateTime NewRefreshTokenExpirationDate()
        {
            var refreshLifetime = _config["Jwt:RefreshLifetimeInMinutes"];
            if(string.IsNullOrEmpty(refreshLifetime))
                throw new ArgumentNullException("Jwt:RefreshLifetimeInMinutes");

            return DateTime.UtcNow.AddMinutes(Int32.Parse(refreshLifetime));
        }

        public async Task<JwtPair> RefreshToken(string refreshToken)
        {
            var tokenEntity = await _rep.Token.FindByTokenWithAccount(refreshToken, true).FirstOrDefaultAsync();
            
            if(tokenEntity == null)
                throw new Exception("Invalid refresh token");

            if(tokenEntity.Expires < DateTime.UtcNow)
                throw new Exception("Refresh token expired");

            var pair = CreatePair(tokenEntity.Account);

            var refreshLifetime = _config["Jwt:RefreshLifetimeInMinutes"];
            if(string.IsNullOrEmpty(refreshLifetime))
                throw new ArgumentNullException("Jwt:RefreshLifetimeInMinutes");

            tokenEntity.RefreshToken = pair.RefreshToken;
            tokenEntity.Expires = DateTime.UtcNow.AddMinutes(Int32.Parse(refreshLifetime));

            await _rep.Save();

            return pair;
        }

        public async Task Revoke(string refreshToken)
        {
            var tokenEntity = await _rep.Token.FindByToken(refreshToken, false).FirstOrDefaultAsync();

            if(tokenEntity == null)
                throw new Exception("Invalid refresh token");

            _rep.Token.Delete(tokenEntity);

            await _rep.Save();
        }
    }
}