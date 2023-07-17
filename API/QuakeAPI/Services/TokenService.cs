using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using QuakeAPI.Data.Models;
using QuakeAPI.Data.Repository.Interfaces;
using QuakeAPI.DTO;
using QuakeAPI.Exceptions;
using QuakeAPI.Options;
using QuakeAPI.Services.Interfaces;

namespace QuakeAPI.Services
{
    public class TokenService : ITokenService
    {
        private readonly IRepositoryManager _rep;
        private readonly JwtOptions _jwtOptions;

        public TokenService(IRepositoryManager rep, IOptions<JwtOptions> jwtOptions)
        {
            _rep = rep;
            _jwtOptions = jwtOptions.Value;
        }

        public JwtPair CreatePair(Account account)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, account.Id.ToString()),
                new Claim(ClaimTypes.Role, account.Role),
            };

            var accessToken = CreateToken(claims, DateTime.UtcNow.AddMinutes(_jwtOptions.AccessLifetimeInMinutes), _jwtOptions.AccessSecret);
            var refreshToken = CreateToken(claims, DateTime.UtcNow.AddMinutes(_jwtOptions.RefreshLifetimeInMinutes), _jwtOptions.RefreshSecret);
            
            return new JwtPair
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }

        public string CreateToken(List<Claim> claims, DateTime expires, string secret)
        {
            var issuer = _jwtOptions.Issuer;
            var audience = _jwtOptions.Audience;
            var key = Encoding.ASCII.GetBytes(secret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
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

        public string GetClaim(string token, string claimType)
        {
            throw new NotImplementedException();
        }

        public async Task<JwtPair> RefreshToken(string refreshToken)
        {
            var tokenEntity = await _rep.Token.FindByTokenWithAccount(refreshToken, true)
                .FirstOrDefaultAsync() ??  throw new NotFoundException("Invalid refresh token");

            if(tokenEntity.Account.DeletedAt != null)
                throw new BadRequestException("You cannot refresh token for deleted account");

            if(tokenEntity.ExpiresAt < DateTime.UtcNow)
                throw new BadRequestException("Refresh token expired");

            var pair = CreatePair(tokenEntity.Account);

            tokenEntity.RefreshToken = pair.RefreshToken;
            tokenEntity.ExpiresAt = DateTime.UtcNow.AddMinutes(_jwtOptions.RefreshLifetimeInMinutes);

            //update last logged date
            tokenEntity.Account.LastLoggedAt = DateTime.UtcNow;

            await _rep.Save();

            return pair;
        }

        public async Task Revoke(string refreshToken)
        {
            var tokenEntity = await _rep.Token.FindByToken(refreshToken, false)
                .FirstOrDefaultAsync() ?? throw new NotFoundException("Invalid refresh token");

            _rep.Token.Delete(tokenEntity);

            await _rep.Save();
        }

        public bool Validate(string token, string secret)
        {
            var validator = new JwtSecurityTokenHandler();

            var validationParams = new TokenValidationParameters
            {  
                ValidIssuer = _jwtOptions.Issuer,
                ValidAudience = _jwtOptions.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(secret)),
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true
            };

            if(validator.CanReadToken(token))
            {
                try
                {
                    validator.ValidateToken(token, validationParams, out var validatedToken);
                    return true;
                }
                catch
                {
                    return false;
                }
            }

            return false;
        }

        /// <summary>
        /// Validate token and return claims
        /// </summary>
        /// <param name="token"></param>
        /// <param name="secret"></param>
        /// <returns>returns null if not valid</returns>
        public ClaimsPrincipal? ValidateWithClaims(string token, string secret)
        {
            var validator = new JwtSecurityTokenHandler();

            var validationParams = new TokenValidationParameters
            {  
                ValidIssuer = _jwtOptions.Issuer,
                ValidAudience = _jwtOptions.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(secret)),
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true
            };

            if(validator.CanReadToken(token))
            {
                try
                {
                    return validator.ValidateToken(token, validationParams, out var validatedToken);
                }
                catch
                {}
            }

            return null;
        }
    }
}