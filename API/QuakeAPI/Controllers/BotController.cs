using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using QuakeAPI.DTO.Telegram;
using Telegram.Bot.Types;

namespace QuakeAPI.Controllers
{
    [ApiController]
    [Route("api/v1/bot")]
    public class BotController : ControllerBase
    {
        [HttpPost("update")]
        public async Task<IActionResult> Update([FromBody] UpdateTg update)
        {
            Console.WriteLine($"Message {update.Message.Text}, id: {update.Message.Id}");
            Console.WriteLine(update.Message.Date);
            return Ok();
        }
    }
}