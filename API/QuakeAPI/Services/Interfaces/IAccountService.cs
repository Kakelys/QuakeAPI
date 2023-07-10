using QuakeAPI.DTO;

namespace QuakeAPI.Services.Interfaces
{
    public interface IAccountService
    {
        Task<List<AccountDto>> GetAll();
        Task<List<AccountDto>> GetPage(Page page);
        Task<JwtPair> Register(Auth auth);
        Task<JwtPair> Login(Auth auth);
        Task Delete(int id);
        Task DeleteOldAccounts();

        Task UpdateUsername(int id, string username);
        Task UpdateEmail(int id, string email);
        Task UpdatePassword(int id, string oldPassword, string newPassword);
    }
}