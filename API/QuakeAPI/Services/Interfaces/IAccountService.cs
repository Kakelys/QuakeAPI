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
    }
}