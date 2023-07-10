using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuakeAPI.Data.Models;
using QuakeAPI.DTO;
using QuakeAPI.Extensions;
using QuakeAPI.Services.Interfaces;

namespace QuakeAPI.Controllers
{
    [ApiController]
    [Route("api/v1/accounts")]
    public class AccountController : ControllerBase
    {
        private IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet, Authorize(Roles = Role.Admin)]
        public async Task<IActionResult> GetAccounts([FromQuery] Page page)
        {
            return Ok(await _accountService.GetPage(page));   
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(Auth auth)
        {
            return Ok(await _accountService.Register(auth));
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(Auth auth)
        {
            return Ok(await _accountService.Login(auth));
        }

        [HttpDelete, Authorize(Roles = Role.User)]
        public async Task<IActionResult> DeleteSelf()
        {
            await _accountService.Delete(User.Id());
            return Ok();
        }

        [HttpDelete("{id}"), Authorize(Roles = Role.Admin)]
        public async Task<IActionResult> Delete(int id)
        {
            await _accountService.Delete(id);
            return Ok();
        }

        [HttpPut, Authorize]
        public async Task<IActionResult> Update(AccountUpdate accountUpdate)
        {
            await _accountService.Update(User.Id(), accountUpdate);
            return Ok();
        }
    }
}