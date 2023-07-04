using QuakeAPI.Data.Models;
using QuakeAPI.DTO;
using QuakeAPI.DTO.Session;
using QuakeAPI.Models.Session;

namespace QuakeAPI.Services.Interfaces
{
    public interface ISessionService
    {
        Task<List<SessionDto>> GetAll();
        Task<List<SessionDto>> GetPage(Page page);
        Task<SessionDetail> GetDetail(int id);
        Task<List<Player>> GetPlayers(int id);
        Task<Session> CreateSession(int accountId, SessionNew session);
        Task ConnectAccount(int sessionId, int accountId);
        Task DisconnectAccount(int sessionId, int accountId);
    }
}