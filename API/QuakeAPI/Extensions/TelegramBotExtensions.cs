using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;

namespace QuakeAPI.Extensions
{
    public static class TelegramBotExtensions
    {
        public static IServiceCollection ConfigureTelegramBot(this IServiceCollection services, IConfiguration config)
        {
            var token = config.GetSection("TelegramBot:Token").Value;
            if (string.IsNullOrEmpty(token))
                throw new ArgumentNullException("TelegramBot:Token");

            var botClient = new TelegramBotClient(token);
            services.AddSingleton(botClient);

            var hook = config.GetSection("TelegramBot:WebhookUrl").Value;
            if(string.IsNullOrEmpty(hook))
                throw new ArgumentNullException("TelegramBot:WebhookUrl");

            try 
            {
                botClient.SetWebhookAsync($"{hook}/api/v1/bot/update").Wait();
            }
            catch(Exception ex)
            {
                services.BuildServiceProvider()?.GetService<ILogger<Program>>()?.LogError(ex, "Failed to set webhook");
            }

            return services;
        }

    }
}