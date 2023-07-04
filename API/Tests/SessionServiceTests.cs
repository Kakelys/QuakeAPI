using Moq;
using QuakeAPI.Data;
using QuakeAPI.Data.Models;
using Microsoft.EntityFrameworkCore;
using QuakeAPI.Data.Repository;
using EntityFrameworkCoreMock;
using QuakeAPI.Data.Repository.Implements;
using QuakeAPI.Services;
using QuakeAPI.DTO.Session;
using QuakeAPI.Exceptions;

namespace Tests
{
    public class SessionServiceTests
    {
        private readonly SessionService _sessionService;

        public SessionServiceTests()
        {
            
            var accounts = new List<Account>
            {
                new Account { Id = 1, Username = "user1", },
                new Account { Id = 2, Username = "user2", }
            };

            var activeAccounts = new List<ActiveAccount>
            {
                new ActiveAccount { AccountId = accounts[0].Id, SessionId = 1, Account = accounts[0]},
                new ActiveAccount { AccountId = accounts[1].Id, SessionId = 2, Account = accounts[1]}
            };

            var sessions = new List<Session>
            {
                new Session 
                    { 
                        Id = 1, 
                        Name = "session1", 
                        MaxPlayers = 10, 
                        LocationId = 1, 
                        Location = new Location { Id = 1, Name = "loc1" }, 
                        ActiveAccounts = new List<ActiveAccount> {activeAccounts[0] }
                    },
                new Session 
                    { 
                        Id = 2, 
                        Name = "session2", 
                        MaxPlayers = 1, 
                        LocationId = 2, 
                        Location = new Location { Id = 2, Name = "loc2" },
                        ActiveAccounts = new List<ActiveAccount> { activeAccounts[1]}
                    },
            };

            activeAccounts[0].Session = sessions[0];
            activeAccounts[1].Session = sessions[1];

            var mockDbContext = new DbContextMock<QuakeDbContext>(
                new DbContextOptionsBuilder<QuakeDbContext>().Options
            );
            mockDbContext.CreateDbSetMock(x => x.Sessions, sessions);
            mockDbContext.CreateDbSetMock(x => x.Accounts, accounts);
            mockDbContext.CreateDbSetMock(x => x.ActiveAccounts, activeAccounts);

            var mockAccountRepo = new Mock<AccountRepository>(mockDbContext.Object);
            var mockLocationRepo = new Mock<LocationRepository>(mockDbContext.Object);
            var mockSessionRepo = new Mock<SessionRepository>(mockDbContext.Object);
            var mockActiveAccountRepo = new Mock<ActiveAccountRepository>(mockDbContext.Object);
            var mockTokenRepo = new Mock<TokenRepository>(mockDbContext.Object);

            var repoManager = new RepositoryManager(
                mockDbContext.Object,
                mockAccountRepo.Object,
                mockLocationRepo.Object,
                mockSessionRepo.Object,
                mockActiveAccountRepo.Object,
                mockTokenRepo.Object
            );

            _sessionService = new SessionService(repoManager);
        }

        [Fact]
        public async Task GetAll_ReturnsSessions()
        {
            var sessions = await _sessionService.GetAll();

            Assert.NotNull(sessions);
            Assert.True(sessions.Count > 0);
        }

        [Fact]
        public void GetDetail_ValidId_ReturnsSessionDetail()
        {
            var session = _sessionService.GetDetail(1);

            Assert.NotNull(session);
            Assert.Equal(1, session.Id);
        }

        [Fact]
        public void GetDetail_InvalidId_ThrowsNotFoundException()
        {
            Assert.ThrowsAsync<NotFoundException>(() => _sessionService.GetDetail(-1));
        }

        [Fact]
        public async Task GetPlayers_ValidId_ReturnsPlayers()
        {
            var players = await _sessionService.GetPlayers(1);

            Assert.NotNull(players);
            Assert.True(players.Count > 0);
        }

        [Fact]
        public void GetPlayers_InvalidId_ThrowsNotFoundException()
        {
            Assert.ThrowsAsync<NotFoundException>(() => _sessionService.GetPlayers(-1));
        }

        [Fact]
        public async Task GetPlayersByPlayer_ValidId_ReturnsPlayers()
        {
            var players = await _sessionService.GetPlayersByPlayer(1);

            Assert.NotNull(players);
            Assert.True(players.Count > 0);
        }

        [Fact]
        public void GetPlayersByPlayer_InvalidId_ThrowsNotFoundException()
        {
            Assert.ThrowsAsync<NotFoundException>(() => _sessionService.GetPlayersByPlayer(-1));
        }

        //Won't work because of the way the mock is set up(many-to-many)
        [Fact]
        public async Task CreateSession_ValidAccountIdAndSession_ReturnsSession()
        {
            var newSession = new SessionNew { Name = "new_session", MaxPlayers = 10, LocationId = 1 };

            var session = await _sessionService.CreateSession(1, newSession);

            Assert.NotNull(session);
        }

        [Fact]
        public void CreateSession_InvalidAccountId_ThrowsNotFoundException()
        {
            var newSession = new SessionNew { Name = "new_session", MaxPlayers = 10, LocationId = 1 };

            Assert.ThrowsAsync<NotFoundException>(() => _sessionService.CreateSession(-1, newSession));
        }

        [Fact]
        public void CreateSession_SessionNull_ThrowsBadRequestException()
        {
            Assert.ThrowsAsync<BadRequestException>(() => _sessionService.CreateSession(1, null));
        }

        //Won't work because of the way the mock is set up(many-to-many)
        [Fact]
        public async Task Connect_ValidAccountIdAndSessionId()
        {
            await _sessionService.AddUser(1, 1);
        }

        [Fact]
        public void Connect_InvalidAccountId_ThrowsNotFoundException()
        {
            Assert.ThrowsAsync<NotFoundException>(() => _sessionService.AddUser(-1, 1));
        }

        [Fact]
        public void Connect_InvalidSessionId_ThrowsNotFoundException()
        {
            Assert.ThrowsAsync<NotFoundException>(() => _sessionService.AddUser(1, -1));
        }

        [Fact]
        public void Connect_SessionFull_ThrowsBadRequestException()
        {
            Assert.ThrowsAsync<BadRequestException>(() => _sessionService.AddUser(2, 1));
        }

        [Fact]
        public async Task Disconnect_ValidAccountIdAndSessionId()
        {
            await _sessionService.RemoveUser(1, 1);
        }

        [Fact]
        public void Disconnect_InvalidAccountId_ThrowsNotFoundException()
        {
            Assert.ThrowsAsync<NotFoundException>(() => _sessionService.RemoveUser(-1, 1));
        }

        [Fact]
        public void Disconnect_InvalidSessionId_ThrowsNotFoundException()
        {
            Assert.ThrowsAsync<NotFoundException>(() => _sessionService.RemoveUser(1, -1));
        }

        [Fact]
        public void Disconnect_AccountNotInSession_ThrowsBadRequestException()
        {
            Assert.ThrowsAsync<BadRequestException>(() => _sessionService.RemoveUser(1, 2));
        }
    }
}