using Microsoft.EntityFrameworkCore;
using QuakeAPI.Data.Repository.Interfaces;
using QuakeAPI.Services.Interfaces;
using QuakeAPI.Data.Models;
using QuakeAPI.Models.Session;
using QuakeAPI.DTO.Session;
using QuakeAPI.DTO;
using QuakeAPI.Exceptions;

namespace QuakeAPI.Services
{
    public class SessionService : ISessionService
    {
        private readonly IRepositoryManager _rep;

        public SessionService(IRepositoryManager rep)
        {
            _rep = rep;
        }

        public async Task<List<SessionDto>> GetAll()
        {
            var sessions = await _rep.Session
                .FindWithAccountsAndLocation()
                .Where(s => s.Deleted != null)
                .Select(session => new SessionDto()
                {
                    Id = session.Id,
                    Name = session.Name,
                    MaxPlayers = session.MaxPlayers,
                    ActivePlayers = session.ActiveAccounts.Count,
                    LocationName = session.Location.Name
                }).ToListAsync();

            return sessions;
        }

        public async Task<SessionDetail> GetDetail(int id)
        {
            var session = await _rep.Session
                .FindWithAccountsAndLocation()
                .Where(s => s.Id == id && s.Deleted != null)
                .Select(session => new SessionDetail()
                {
                    Id = session.Id,
                    Name = session.Name,
                    MaxPlayers = session.MaxPlayers,
                    ActivePlayers = session.ActiveAccounts.Count,
                    Location = session.Location
                })
                .FirstOrDefaultAsync() ?? throw new NotFoundException("Session doesn't exist.");

            return session;
        }

        public async Task<List<Player>> GetPlayers(int id)
        {
            var session = await _rep.Session.FindByCondition(x => x.Id == id, false)
                .Include(x => x.ActiveAccounts)
                .FirstOrDefaultAsync() ?? throw new NotFoundException("Session does not exist.");

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
            ArgumentNullException.ThrowIfNull(session, "Session");

            var creatorAccount = await _rep.Account.FindByCondition(x => x.Id == accountId, false)
                .Include(a => a.ActiveAccount)
                .FirstOrDefaultAsync() ?? throw new NotFoundException("Account does not exist.");

            if(creatorAccount.ActiveAccount != null)
                throw new BadRequestException("Account is already in a session.");

            var sessionEntity = new Session()
            {
                Name = session.Name,
                LocationId = session.LocationId,
                MaxPlayers = session.MaxPlayers,
                Created = DateTime.Now
            };

            var ActiveAccount = new ActiveAccount()
            {
                AccountId = creatorAccount.Id,
                SessionId = sessionEntity.Id,
                Connected = DateTime.Now
            };

            sessionEntity.ActiveAccounts.Add(ActiveAccount);

            _rep.Session.Create(sessionEntity);
            
            await _rep.Save();

            return sessionEntity;
        }

        public async Task AddUser(int sessionId, int accountId)
        {
            var user = await _rep.Account.FindByCondition(x => x.Id == accountId, false)
                .FirstOrDefaultAsync() ?? throw new NotFoundException("Account does not exist.");

            var session = _rep.Session
                .FindByCondition(s => s.Id == sessionId, false)
                .Include(s => s.ActiveAccounts)
                .FirstOrDefault() ?? throw new NotFoundException("Session does not exist.");
            
            if(session.Deleted != null)
                throw new BadRequestException("Session is deleted.");

            if(session.ActiveAccounts.Count >= session.MaxPlayers)
                throw new BadRequestException("Session is full.");

            var activeAccount = new ActiveAccount()
            {
                AccountId = user.Id,
                SessionId = session.Id,
                Connected = DateTime.Now
            };
            
            _rep.ActiveAccount.Create(activeAccount);

            await _rep.Save();
        }

        public async Task RemoveUser(int sessionId, int accountId)
        {
            var session = await _rep.Session.FindByCondition(x => x.Id == sessionId, true)
                .Include(s => s.ActiveAccounts)
                .FirstOrDefaultAsync() ?? throw new NotFoundException("Session does not exist.");

            var activeAccount = session.ActiveAccounts.FirstOrDefault(x => x.AccountId == accountId);
            if(activeAccount == null)
                throw new BadRequestException("Account is not in this session.");

            activeAccount.Disconnected = DateTime.Now;

            if(session.ActiveAccounts.Count == 1)
            {
                session.Deleted = DateTime.Now;
            }

            await _rep.Save();
        }

        public async Task<List<Player>> GetPlayersByPlayer(int accountId)
        {
            var session = await _rep.Session.FindByAccountId(accountId, false)
                .FirstOrDefaultAsync() ?? throw new NotFoundException("Session does not exist.");

            var players = session.ActiveAccounts.Select(x => new Player()
            {
                Id = x.Account.Id,
                Name = x.Account.Email,
            }).ToList();
            
            return players;
        }
    }
}