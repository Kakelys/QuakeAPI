using Microsoft.EntityFrameworkCore;
using QuakeAPI.Data.Models;
using QuakeAPI.Data.Repository.Interfaces;
using QuakeAPI.DTO;
using QuakeAPI.Exceptions;
using QuakeAPI.Extensions;
using QuakeAPI.Services.Interfaces;

namespace QuakeAPI.Services
{
    public class AccountService : IAccountService
    {
        private IRepositoryManager _rep;
        private ITokenService _tokenService;
        private IConfiguration _config;

        public AccountService(IRepositoryManager rep, ITokenService tokenService, IConfiguration config)
        {
            _rep = rep;
            _tokenService = tokenService;
            _config = config;
        }

        public async Task<JwtPair> Login(Auth auth)
        {
            var account = await _rep.Account.FindByEmailWithTokens(auth.Email, true)
                .FirstOrDefaultAsync() ?? throw new NotFoundException("Account with that email not found");

            if(!PasswordHelper.VerifyPassword(auth.Password, account.PasswordHash))
                throw new BadRequestException("Wrong password");

            var pair = _tokenService.CreatePair(account);
            var maxTokenString = _config["Jwt:MaxTokenCount"];
            if(string.IsNullOrEmpty(maxTokenString))
                throw new NullReferenceException("Jwt:MaxTokenCount");

            var maxTokenCount = int.Parse(maxTokenString);

            if(account.Tokens.Count >= maxTokenCount)
            {
                var oldestToken = account.Tokens.OrderBy(t => t.Expires).First();
                account.Tokens.Remove(oldestToken);
            }

            account.Tokens.Add(new Token
            {
                AccountId = account.Id,
                RefreshToken = pair.RefreshToken,
                Expires = _tokenService.NewRefreshTokenExpirationDate()
            });

            await _rep.Save();
            
            return pair;
        }

        public async Task<JwtPair> Register(Auth auth)
        {
            //create account
            if(await _rep.Account.FindByEmail(auth.Email, false).FirstOrDefaultAsync() != null)
                throw new BadRequestException("Account with same email already exists");

            var account = new Account
            {
                Email = auth.Email,
                PasswordHash = PasswordHelper.HashPassword(auth.Password)
            };

            await _rep.BeginTransaction();
            try 
            {
                _rep.Account.Create(account);
                await _rep.Save();

                //generate pair
                var pair = _tokenService.CreatePair(account);

                //save to db
                account.Tokens.Add(new Token
                {
                    AccountId = account.Id,
                    RefreshToken = pair.RefreshToken,
                    Expires = _tokenService.NewRefreshTokenExpirationDate()
                });

                await _rep.Save();
                await _rep.Commit();

                return pair;
            }
            catch
            {
                await _rep.Rollback();
                throw;
            }
        }

        public async Task Delete(int id)
        {
            var user = await _rep.Account.FindById(id, false)
                .FirstOrDefaultAsync() ?? throw new NotFoundException("Account not found");

            _rep.Account.Delete(user);

            await _rep.Save();
        }

        public async Task<List<AccountDto>> GetAll()
        {
            return await _rep.Account.FindAll(false).Select(a => new AccountDto
            {
                Id = a.Id,
                Email = a.Email,
                Username = a.Username,
                Role = a.Role
            }).ToListAsync();
        }

        public async Task<List<AccountDto>> GetPage(Page page)
        {
            return await _rep.Account.FindPage(page, false)
            .Select(a => new AccountDto
            {
                Id = a.Id,
                Email = a.Email,
                Username = a.Username,
                Role = a.Role
            }).ToListAsync();
        }
    }
}