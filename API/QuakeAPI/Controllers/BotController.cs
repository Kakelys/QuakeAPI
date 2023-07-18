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
        private readonly ILogger<BotController> _logger;

        public BotController(
            ITelegramService telegramService,
            ILogger<BotController> logger)
        {
            _telegramService = telegramService;
            _logger = logger;
        }

        [HttpPost("update")]
        public async Task<IActionResult> Update([FromBody] Update update)
        {    
            // try/catch nedded to prevent telegram from sending same message twice
            try
            {
                await _telegramService.ProcessMessage(update);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Failed to process telegram message");
            }

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