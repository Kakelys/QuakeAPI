using QuakeAPI.DTO.Telegram;

namespace QuakeAPI.Services.Interfaces
{
    public interface ITelegramService
    {
        Task ProcessMessage(Update update);
        Task SendMessage(int chatId, string message);
        Task ConfirmLink(string linkToken);
    }
}