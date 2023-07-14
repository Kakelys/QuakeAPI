using QuakeAPI.DTO;
using QuakeAPI.DTO.Account;

namespace QuakeAPI.Services.Interfaces
{
    public interface IAccountService
    {
        Task<List<AccountDto>> GetAll();
        Task<List<AccountDto>> GetPage(Page page);
        Task<List<AccountDto>> GetPageWithSerach(Page page, AccountSearch accountSearch);
        Task<JwtPair> Register(Auth auth);
        Task<JwtPair> Login(Auth auth);
        Task Delete(int id);
        Task DeleteOldAccounts();
        Task Update(int id, AccountUpdate accountUpdate);
    }
}