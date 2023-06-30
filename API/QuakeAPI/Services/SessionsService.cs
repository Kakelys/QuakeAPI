using Microsoft.EntityFrameworkCore;
using QuakeAPI.Data.Repository.Interfaces;
using QuakeAPI.Services.Interfaces;
using QuakeAPI.Data.Models;
using QuakeAPI.Models.Session;
using QuakeAPI.DTO.Session;
using QuakeAPI.DTO;

namespace QuakeAPI.Services
{
    public class SessionsService : ISessionService
    {
        private IRepositoryManager _rep;

        public SessionsService(IRepositoryManager rep)
        {
            _rep = rep;
        }

        public async Task<List<SessionDto>> GetAll()
        {
            var sessions = await _rep.Session.FindAll(false)
                .Include(x => x.Location)
                .Include(x => x.ActiveAccounts)
                .Select(session => new SessionDto()
                {
                    Id = session.Id,
                    Name = session.Name,
                    MaxPlayers = session.ActiveAccounts.Count,
                    LocationName = session.Location.Name
                }).ToListAsync();

            return sessions;
        }

        public async Task<SessionDetail> GetDetail(int id)
        {
            var session = await _rep.Session.FindByCondition(x => x.Id == id, false)
                .Include(x => x.Location)
                .Include(x => x.ActiveAccounts)
                .Select(session => new SessionDetail()
                {
                    Id = session.Id,
                    Name = session.Name,
                    MaxPlayers = session.MaxPlayers,
                    ActivePlayers = session.ActiveAccounts.Count,
                    Location = session.Location
                }).FirstOrDefaultAsync();

            if(session == null)
                throw new Exception("Session does not exist.");

            return session;
        }

        public async Task<List<Player>> GetPlayers(int id)
        {
            var session = await _rep.Session.FindByCondition(x => x.Id == id, false)
                .Include(x => x.ActiveAccounts)
                .FirstOrDefaultAsync();

            if(session == null)
                throw new Exception("Session does not exist.");

            var players = session.ActiveAccounts.Select(x => new Player() 
            {
                Id = x.Account.Id,
                Name = x.Account.Email,
            })
            .ToList();

            return players;
        }

        public async Task<Session> CreateSession(int accountId, SessionNew session)
        {
            var creatorAccount = await _rep.Account.FindByCondition(x => x.Id == accountId, false).FirstOrDefaultAsync();
            if(creatorAccount == null)
                throw new Exception("Account does not exist.");

            var sessionEntity = new Session()
            {
                Name = session.Name,
                LocationId = session.LocationId,
                MaxPlayers = session.MaxPlayers,
                ActiveAccounts = new List<ActiveAccount>()
            };

            var ActiveAccount = new ActiveAccount()
            {
                AccountId = creatorAccount.Id,
                SessionId = sessionEntity.Id
            };

            sessionEntity.ActiveAccounts.Add(ActiveAccount);

            _rep.Session.Create(sessionEntity);

            await _rep.Save();

            return sessionEntity;
        }

        public async Task AddUser(int sessionId, int accountId)
        {
            var user = _rep.Account.FindByCondition(x => x.Id == accountId, false).FirstOrDefault();
            if(user == null)
                throw new Exception("Account does not exist.");

            var session = _rep.Session.FindByCondition(x => x.Id == sessionId, false).FirstOrDefault();
            if(session == null)
                throw new Exception("Session does not exist.");

            var activeAccount = new ActiveAccount()
            {
                AccountId = user.Id,
                SessionId = session.Id
            };
            
            _rep.ActiveAccount.Create(activeAccount);

            await _rep.Save();
        }

        public async Task RemoveUser(int sessionId, int accountId)
        {
            var session = _rep.Session.FindByCondition(x => x.Id == sessionId, false)
                .Include(s => s.ActiveAccounts)
                .FirstOrDefault();

            if(session == null)
                throw new Exception("Session does not exist.");

            var activeAccount = session.ActiveAccounts.FirstOrDefault(x => x.AccountId == accountId);
            if(activeAccount == null)
                throw new Exception("Account is not in this session.");

            if(session.ActiveAccounts.Count == 1)
            {
                _rep.Session.Delete(session);
            }
            else
            {
                _rep.ActiveAccount.Delete(activeAccount);
            }

            await _rep.Save();
        }

        public async Task<List<Player>> GetPlayersByPlayer(int accountId)
        {
            var session = await _rep.Session.FindByAccountId(accountId, false).FirstOrDefaultAsync();
            if(session == null)
                throw new Exception("Session does not exist.");

            var players = session.ActiveAccounts.Select(x => new Player()
            {
                Id = x.Account.Id,
                Name = x.Account.Email,
            });
            
            return players.ToList();
        }
    }
}