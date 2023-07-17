using Microsoft.AspNetCore.Mvc;
using QuakeAPI.DTO.Telegram;
using QuakeAPI.Services.Interfaces;

namespace QuakeAPI.Controllers
{
    [ApiController]
    [Route("api/v1/bot")]
    public class BotController : ControllerBase
    {
        private readonly ITelegramService _telegramService;

        public BotController(ITelegramService telegramService)
        {
            _telegramService = telegramService;
        }

        [HttpPost("update")]
        public async Task<IActionResult> Update([FromBody] Update update)
        {
            Console.WriteLine($"Message {update.Message.Text}, id: {update.Message.Id}");
            Console.WriteLine(update.Message.Date);
            Console.WriteLine(update.Id);
            
            await _telegramService.ProcessMessage(update);

            return Ok();
        }

        [HttpGet("link/{linkToken}")]
        public async Task<IActionResult> ConfirmLink(string linkToken)
        {
            Console.WriteLine(linkToken);
            await _telegramService.ConfirmLink(linkToken);

            return Ok();
        }
    }
}