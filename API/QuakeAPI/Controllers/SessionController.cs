using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuakeAPI.Data.Models;
using QuakeAPI.DTO;
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
        public async Task<IActionResult> GetSessions([FromQuery] Page page)
        {
            return Ok(await _sessionService.GetPage(page));   
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

        [HttpPost, Authorize(Roles = Role.User)]
        public async Task<IActionResult> CreateSession(SessionNew session)
        {
            return Ok(await _sessionService.CreateSession(User.Id(),session));
        }

        [HttpPost("{id}/connect"), Authorize(Roles = Role.User)]
        public async Task<IActionResult> ConnectToSession(int id)
        {
            await _sessionService.ConnectAccount(id, User.Id());
            return Ok();
        }

        [HttpPost("{id}/disconnect"), Authorize(Roles = Role.User)]
        public async Task<IActionResult> DisconnectFromSession(int id)
        {
            await _sessionService.DisconnectAccount(id, User.Id());
            return Ok();
        }
    }
}