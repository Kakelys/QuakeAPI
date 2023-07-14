using System.Net;
using System.Net.Mail;
using QuakeAPI.Services.Interfaces;

namespace QuakeAPI.Services
{
    public class EmailService : IEmailService
    {
        private readonly ILogger<EmailService> _logger;
        private const string fromAddress = "seno.v.test@gmail.com";
        private const string fromPassword = "saozemqwwysggaog";

        public EmailService(ILogger<EmailService> logger)
        {
            _logger = logger;
        }

        public async Task Send(string to, string subject, string body)
        {
            var smtp = new SmtpClient("smtp.gmail.com", 587)
            {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    Credentials = new NetworkCredential(fromAddress, fromPassword),
                    Timeout = 20000
            };

            try
            {
                using var message = new MailMessage(fromAddress, to)
                {
                    Subject = subject,
                    Body = body
                };

                await smtp.SendMailAsync(message);
            } 
            catch (Exception ex)
            {
                _logger.LogError($"Failed to send Email: {ex.Message}");
                throw ex;
            } 
        }
    }
}