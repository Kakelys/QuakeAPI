using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuakeAPI.Data.Models;
using QuakeAPI.DTO.Session;
using QuakeAPI.Extensions;
using QuakeAPI.Services.Interfaces;

namespace QuakeAPI.Controllers
{
    [ApiController]
    [Route("api/v1/sessions")]
    public class SessionController : ControllerBase
    {
        private ISessionService _sessionService;

        public SessionController(ISessionService sessionService)
        {
            _sessionService = sessionService;
        }

        [HttpGet, Authorize(Roles = $"{Role.Admin},{Role.User}")]
        public async Task<IActionResult> GetSessions()
        {
            return Ok(await _sessionService.GetAll());   
        }
        
        [HttpGet("{id}"), Authorize(Roles = $"{Role.Admin},{Role.User}")]
        public async Task<IActionResult> GetSessionDetail(int id)
        {
            return Ok(await _sessionService.GetDetail(id));
        }

        [HttpGet("{id}/players"), Authorize(Roles = Role.User)]
        public async Task<IActionResult> GetSessionPlayers(int id)
        {
            return Ok(await _sessionService.GetPlayers(id));
        }

        [HttpGet("players"), Authorize(Roles = Role.User)]
        public async Task<IActionResult> GetPlayerSessions()
        {
            return Ok(await _sessionService.GetPlayersByPlayer(User.Id()));
        }

        [HttpPost, Authorize(Roles = Role.User)]
        public async Task<IActionResult> CreateSession(SessionNew session)
        {
            return Ok(await _sessionService.CreateSession(User.Id(),session));
        }

        [HttpPost("{id}/connect"), Authorize(Roles = Role.User)]
        public async Task<IActionResult> ConnectToSession(int id)
        {
            await _sessionService.AddUser(id, User.Id());
            return Ok();
        }

        [HttpPost("{id}/disconnect"), Authorize(Roles = Role.User)]
        public async Task<IActionResult> DisconnectFromSession(int id)
        {
            await _sessionService.RemoveUser(id, User.Id());
            return Ok();
        }
    }
}