using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using QuakeAPI.Data.Models;
using QuakeAPI.Data.Repository.Interfaces;
using QuakeAPI.DTO;
using QuakeAPI.Exceptions;
using QuakeAPI.Extensions;
using QuakeAPI.Options;
using QuakeAPI.Services.Interfaces;

namespace QuakeAPI.Services
{
    public class AccountService : IAccountService
    {
        private readonly IRepositoryManager _rep;
        private readonly ITokenService _tokenService;
        private readonly JwtOptions _jwtOptions;
        private readonly IEmailService _emailService;
        private readonly ILogger<AccountService> _logger;

        public AccountService(
            IRepositoryManager rep, 
            ITokenService tokenService, 
            IOptions<JwtOptions> jwtOptions,
            IEmailService emailService,
            ILogger<AccountService> logger)
        {
            _rep = rep;
            _tokenService = tokenService;
            _jwtOptions = jwtOptions.Value;
            _emailService = emailService;
            _logger = logger;
        }

        public async Task<JwtPair> Login(Auth auth)
        {
            var account = await _rep.Account.FindByEmailWithTokens(auth.Email, true)
                .FirstOrDefaultAsync() ?? throw new NotFoundException("Account with that email not found");

            if(account.DeletedAt != null)
                throw new BadRequestException("Account is deleted");

            if(!PasswordHelper.VerifyPassword(auth.Password, account.PasswordHash))
                throw new BadRequestException("Wrong password");

            //update tokens
            var pair = _tokenService.CreatePair(account);
            var maxTokenCount = _jwtOptions.MaxTokenCount;

            if(account.Tokens.Count >= maxTokenCount)
            {
                var oldestToken = account.Tokens.OrderBy(t => t.ExpiresAt).First();
                account.Tokens.Remove(oldestToken);
            }

            account.Tokens.Add(new Token
            {
                AccountId = account.Id,
                RefreshToken = pair.RefreshToken,
                ExpiresAt = DateTime.UtcNow.AddMinutes(_jwtOptions.RefreshLifetimeInMinutes)
            });

            //update last logged date
            account.LastLoggedAt = DateTime.UtcNow;

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
                PasswordHash = PasswordHelper.HashPassword(auth.Password),
                LastLoggedAt = DateTime.UtcNow,
            };

            //transaction needed because token generation needs account id
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
                    ExpiresAt = DateTime.UtcNow.AddMinutes(_jwtOptions.RefreshLifetimeInMinutes)
                });

                await _rep.Save();
                await _rep.Commit();

                try
                {
                    _emailService.Send(auth.Email, "Quake API - Registration", "You have been registered");
                } catch(Exception ex)
                {
                    _logger.LogError(ex, "Failed to send email");
                }

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
            var user = await _rep.Account.FindById(id, true)
                .FirstOrDefaultAsync() ?? throw new NotFoundException("Account not found");

            _rep.Account.Delete(user);

            await _rep.Save();

            try
            {
                _emailService.Send(user.Email, "Quake API - Account deleted", "Your account has been deleted");
            } catch(Exception ex)
            {
                _logger.LogError(ex, "Failed to send email");
            }
        }

        public async Task<List<AccountDto>> GetAll()
        {
            return await _rep.Account
            .FindNotDeleted(false)
            .Select(a => new AccountDto
            {
                Id = a.Id,
                Email = a.Email,
                Username = a.Username,
                Role = a.Role
            }).ToListAsync();
        }

        public async Task<List<AccountDto>> GetPage(Page page)
        {
            return await _rep.Account
            .FindNotDeleted(false)
            .TakePage(page)
            .Select(a => new AccountDto
            {
                Id = a.Id,
                Email = a.Email,
                Username = a.Username,
                Role = a.Role
            }).ToListAsync();
        }

        public async Task Update(int id, AccountUpdate accountUpdate)
        {
            var account = await _rep.Account.FindById(id, true)
                .FirstOrDefaultAsync() ?? throw new NotFoundException("Account not found");

            account.Username = accountUpdate.Username ?? account.Username;
            
            if(account.Email != accountUpdate.Email && await _rep.Account.FindByEmail(accountUpdate.Email, false).AnyAsync())
            {
                throw new BadRequestException("Account with same email already exists");
            }
            else
                account.Email = accountUpdate.Email ?? account.Email;
            
            if(
                !string.IsNullOrEmpty(accountUpdate.NewPassword) 
                && !string.IsNullOrEmpty(accountUpdate.OldPassword) 
                && PasswordHelper.VerifyPassword(accountUpdate.OldPassword, account.PasswordHash)
                )
                account.PasswordHash = PasswordHelper.HashPassword(accountUpdate.NewPassword);

            await _rep.Save();
        }

        /// <summary>
        /// Deletes accounts that haven't logged in for 3 months
        /// </summary>
        /// <returns></returns>
        public async Task DeleteOldAccounts()
        {
            var users = await _rep.Account
                .FindByCondition(a => a.LastLoggedAt < DateTime.UtcNow.AddMonths(-3), true)
                .ToListAsync();

            _rep.Account.DeleteMany(users);

            await _rep.Save();
        }
    }
}