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
            var timeout = 5000;

            //smtp timeout only work with sync send
            var smtp = new SmtpClient("smtp.gmail.com", 587)
            {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    Credentials = new NetworkCredential(fromAddress, fromPassword),
            };

            try
            {
                using var message = new MailMessage(fromAddress, to)
                {
                    Subject = subject,
                    Body = body
                };

                //checking if message sended in time
                var messageSended = false;
                smtp.SendCompleted += (s, e) => {
                    messageSended = true;
                };
                var timeoutTask = Task.Run(async () => {
                    await Task.Delay(timeout);
                    if(!messageSended)
                    {
                        smtp.SendAsyncCancel();
                    }     
                });

                _logger.LogInformation($"Sending email to {to}");
                await smtp.SendMailAsync(message);
                _logger.LogInformation($"Email sended to {to}");
            } 
            catch(TaskCanceledException ex)
            {
                _logger.LogError($"Timeout while sending Email: {ex.Message}");
                throw ex;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to send Email: {ex.Message}");
                throw ex;
            } 
        }
    }
}