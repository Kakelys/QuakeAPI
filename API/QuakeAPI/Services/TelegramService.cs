using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using QuakeAPI.Data.Repository.Interfaces;
using QuakeAPI.DTO.Telegram;
using QuakeAPI.Exceptions;
using QuakeAPI.Options;
using QuakeAPI.Services.Interfaces;
using Telegram.Bot;

namespace QuakeAPI.Services
{
    public class TelegramService : ITelegramService
    {
        private readonly ITokenService _tokenService;
        private readonly IEmailService _emailService;
        private readonly IRepositoryManager _rep;
        private readonly TelegramBotClient _bot;
        private readonly JwtOptions _jwtOptions;

        public TelegramService(
            ITokenService tokenService,
            IEmailService emailService,
            TelegramBotClient bot,
            IRepositoryManager rep,
            IOptions<JwtOptions> jwtOptions)
        {
            _tokenService = tokenService;    
            _emailService = emailService;
            _rep = rep;
            _bot = bot;
            _jwtOptions = jwtOptions.Value;
        }

        public async Task ProcessMessage(Update update)
        {
            var command = update.Message.Text.Trim().Split(' ').First();
            switch (command)
            {
                case "/start":
                    await _bot.SendTextMessageAsync(update.Message.Chat.Id, "Hello, I'm QuakeBot;\nTo start getting notifications, please link your account with: \n```/link example@gmail.com```\nOR```/link account_id```", parseMode: Telegram.Bot.Types.Enums.ParseMode.Markdown);
                    break;
                case "/link":
                    await OnLinkAccount(update.Message);
                    break;
                case "/stop":
                    await OnStopNotifications(update.Message.Chat.Id);
                    break;
                default:
                    await _bot.SendTextMessageAsync(update.Message.Chat.Id, "Unknown command");
                    break;
            }
        }
        
        public async Task ConfirmLink(string linkToken)
        {
            var claims = _tokenService
                .ValidateWithClaims(linkToken, _jwtOptions.TgSecret);
            if(claims == null)
                throw new BadRequestException("Invalid token");

            int.TryParse(claims.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)?.Value, out var accountId);
            int.TryParse(claims.FindFirst(c => c.Type == "chatid")?.Value, out var chatId);

            if(accountId == 0 || chatId == 0)
                throw new BadRequestException("Invalid token");
            
            var account = await _rep.Account
                .FindNotDeleted(true)
                .FirstOrDefaultAsync(a => a.Id == accountId) ?? throw new BadRequestException("Account not found");

            account.TelegramChatId = chatId;

            await _rep.Save();

            await _bot.SendTextMessageAsync(chatId, "Account linked successfully");
        }

        public async Task SendMessage(int chatId, string message)
        {
            await _bot.SendTextMessageAsync(chatId, message);
        }

        private async Task OnLinkAccount(Message message)
        {
            var userIdentifier = message.Text.Split(' ').Last();
            int.TryParse(userIdentifier, out var accountId);
            
            var account = await _rep.Account
                .FindNotDeleted(false)
                .Where(a => a.Id == accountId || a.Email == userIdentifier)
                .FirstOrDefaultAsync();

            if(account == null)
            {
                await _bot.SendTextMessageAsync(message.Chat.Id, "Account not found");
                return;
            }

            //create token
            var linkToken = _tokenService.CreateToken(new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, account.Id.ToString()),
                new Claim(ClaimTypes.Role, "telegram"),
                new Claim("chatid", message.Chat.Id.ToString())
            }, DateTime.UtcNow.AddMinutes(_jwtOptions.TgLifetimeInMinutes), _jwtOptions.TgSecret);

            //send email
            await _emailService.Send(account.Email, "Link your account", $"Please follow this link to link your account with telegram botю \nLink will expire in {_jwtOptions.TgLifetimeInMinutes} minutes\n Link:\n {_jwtOptions.Issuer}/api/v1/bot/link/{linkToken}");

            await _bot.SendTextMessageAsync(message.Chat.Id, "Now check email and follow the instructions");
        }
        
        private async Task OnStopNotifications(int chatId)
        {
            var account = await _rep.Account
                .FindNotDeleted(true)
                .FirstOrDefaultAsync(a => a.TelegramChatId == chatId) ?? throw new BadRequestException("Account not found");

            account.TelegramChatId = 0;

            await _rep.Save();
        }
    }
}