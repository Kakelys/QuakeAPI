using QuakeAPI.Data.Models;
using QuakeAPI.DTO;
using QuakeAPI.DTO.Session;
using QuakeAPI.Models.Session;

namespace QuakeAPI.Services.Interfaces
{
    public interface ISessionService
    {
        Task<List<SessionDto>> GetAll();
        Task<SessionDetail> GetDetail(int id);
        Task<List<Player>> GetPlayers(int id);
        Task<List<Player>> GetPlayersByPlayer(int accountId);
        Task<Session> CreateSession(int accountId, SessionNew session);
        Task AddUser(int sessionId, int accountId);
        Task RemoveUser(int sessionId, int accountId);
    }
}