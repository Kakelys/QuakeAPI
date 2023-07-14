using Microsoft.EntityFrameworkCore;
using QuakeAPI.Data.Repository.Interfaces;
using QuakeAPI.Services.Interfaces;
using QuakeAPI.Data.Models;
using QuakeAPI.Models.Session;
using QuakeAPI.DTO.Session;
using QuakeAPI.DTO;
using QuakeAPI.Exceptions;
using QuakeAPI.Extensions;
using Microsoft.Identity.Client;

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
            return await _rep.Session
                .FindWithAccountsAndLocation()
                .Select(session => new SessionDto()
                {
                    Id = session.Id,
                    Name = session.Name,
                    MaxPlayers = session.MaxPlayers,
                    ActivePlayers = session.ActiveAccounts.Count,
                    LocationName = session.Location.Name
                }).ToListAsync();
        }

        public async Task<List<SessionDto>> GetPage(Page page)
        {
            return await _rep.Session
                .FindWithAccountsAndLocation()
                .TakePage(page)
                .Select(session => new SessionDto()
                {
                    Id = session.Id,
                    Name = session.Name,
                    MaxPlayers = session.MaxPlayers,
                    ActivePlayers = session.ActiveAccounts.Count,
                    LocationName = session.Location.Name
                }).ToListAsync();
        }

        public async Task<SessionDetail> GetDetail(int id)
        {
            var session = await _rep.Session
                .FindWithAccountsAndLocation()
                .Where(s => s.Id == id && s.DeletedAt == null)
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
                .Include(x => x.ActiveAccounts.Where(x => x.DisconnectedAt == null))
                .ThenInclude(x => x.Account)
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

            var creatorAccount = await _rep.Account
                .FindByCondition(x => x.Id == accountId, false)
                .Include(a => a.ActiveAccounts.Where(a => a.DisconnectedAt == null))
                .FirstOrDefaultAsync() ?? throw new NotFoundException("Account does not exist.");

            var location = await _rep.Location
                .FindByCondition(x => x.Id == session.LocationId, false)
                .FirstOrDefaultAsync() ?? throw new NotFoundException("Location does not exist.");

            if(location.MaxPlayers < session.MaxPlayers)
                throw new BadRequestException($"Session max players is higher than location max players. Limit: {location.MaxPlayers}");

            if(creatorAccount.ActiveAccounts.Count > 0)
                throw new BadRequestException("Account is already in a session.");

            var sessionEntity = new Session()
            {
                Name = session.Name,
                LocationId = session.LocationId,
                MaxPlayers = session.MaxPlayers,
                CreatedAt = DateTime.Now
            };

            var ActiveAccount = new ActiveAccount()
            {
                AccountId = creatorAccount.Id,
                SessionId = sessionEntity.Id,
                ConnectedAt = DateTime.Now
            };

            sessionEntity.ActiveAccounts.Add(ActiveAccount);

            _rep.Session.Create(sessionEntity);
            
            await _rep.Save();

            return sessionEntity;
        }

        public async Task ConnectAccount(int sessionId, int accountId)
        {
            var user = await _rep.Account
                .FindByCondition(x => x.Id == accountId, false)
                .FirstOrDefaultAsync() ?? throw new NotFoundException("Account does not exist.");

            var session = _rep.Session
                .FindByCondition(s => s.Id == sessionId, false)
                .Include(s => s.ActiveAccounts.Where(s => s.DisconnectedAt == null))
                .FirstOrDefault() ?? throw new NotFoundException("Session does not exist.");
            
            if(session.DeletedAt != null)
                throw new BadRequestException("Session is deleted.");

            if(session.ActiveAccounts.Count >= session.MaxPlayers)
                throw new BadRequestException("Session is full.");

            if(session.ActiveAccounts.Where(x => x.AccountId == accountId).Count() > 0)
                throw new BadRequestException("Account is already in this session.");

            var activeAccount = new ActiveAccount()
            {
                AccountId = user.Id,
                SessionId = session.Id,
                ConnectedAt = DateTime.Now
            };
            
            _rep.ActiveAccount.Create(activeAccount);

            await _rep.Save();
        }

        public async Task DisconnectAccount(int sessionId, int accountId)
        {
            var session = await _rep.Session.FindByCondition(x => x.Id == sessionId, true)
                .Include(s => s.ActiveAccounts.Where(s => s.DisconnectedAt == null))
                .FirstOrDefaultAsync() ?? throw new NotFoundException("Session does not exist.");

            var activeAccount = session.ActiveAccounts.FirstOrDefault(aa => aa.AccountId == accountId);
            if(activeAccount == null)
                throw new BadRequestException("Account is not in this session.");

            activeAccount.DisconnectedAt = DateTime.Now;

            if(session.ActiveAccounts.Count == 1)
            {
                session.DeletedAt = DateTime.Now;
            }

            await _rep.Save();
        }
    }
}