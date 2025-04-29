using System;
using System.Threading.Tasks;
using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;

namespace Demo03.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(string to, string subject, string body);
    }

    public class EmailService : IEmailService
    {
        private readonly string _smtpServer;
        private readonly int _smtpPort;
        private readonly string _fromEmail;
        private readonly string _fromPassword;

        public EmailService(IConfiguration configuration)
        {
            _smtpServer = configuration["EmailSettings:SmtpServer"];
            _smtpPort = int.Parse(configuration["EmailSettings:SmtpPort"]);
            _fromEmail = configuration["EmailSettings:FromEmail"];
            _fromPassword = configuration["EmailSettings:FromPassword"];
        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            try
            {
                var message = new MailMessage();
                message.From = new MailAddress(_fromEmail);
                message.Subject = subject;
                message.To.Add(new MailAddress(to));
                message.Body = body;
                message.IsBodyHtml = true;

                using var client = new SmtpClient(_smtpServer, _smtpPort);
                client.EnableSsl = true;
                client.Credentials = new NetworkCredential(_fromEmail, _fromPassword);

                await client.SendMailAsync(message);
            }
            catch (Exception ex)
            {
                // Log the error but don't throw it to prevent revealing sensitive info
                Console.WriteLine($"Error sending email: {ex.Message}");
            }
        }
    }
} 