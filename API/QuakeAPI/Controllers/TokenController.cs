using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuakeAPI.Data.Models;
using QuakeAPI.Data.Repository.Interfaces;
using QuakeAPI.Extensions;
using QuakeAPI.Services.Interfaces;

namespace QuakeAPI.Controllers
{
    [ApiController]
    [Route("api/v1/tokens")]
    public class TokenController : ControllerBase
    {
        private ITokenService _tokenService;

        public TokenController(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }

        [Authorize]
        [HttpDelete("{token}")]
        public async Task<IActionResult> Revoke(string token)
        {
            await _tokenService.Revoke(token);
            return Ok();
        }

        [HttpGet("test")]
        public IActionResult Test()
        {
            return Ok(_tokenService.CreatePair(new Account
            {
                Id = 1,
                Role = Role.User
            }));
        }

        [HttpGet("test-auth"), Authorize]
        public IActionResult TestAuth()
        {
            return Ok(User.Id());
        }

        [HttpGet("test-auth-user"), Authorize(Roles = Role.User)]
        public IActionResult TestAuthUser()
        {
            return Ok();
        }

        [HttpGet("test-auth-admin"), Authorize(Roles = Role.Admin)]
        public IActionResult TestAuthAdmin()
        {
            return Ok();
        }
    }
}