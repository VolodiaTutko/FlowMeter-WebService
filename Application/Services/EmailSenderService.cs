using Application.Models;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Application.Services
{
    public class EmailSenderService : IEmailSenderService
    {
        private readonly IConfiguration _config;
        private readonly SmtpClient _smtpClient;
        private readonly SmtpSettings _smtpSettings;
        private readonly Random _random;

        public EmailSenderService(IConfiguration config, SmtpSettings smtpSettings)
        {
            _config = config;
            _smtpSettings = smtpSettings;
            _random = new Random();
            _smtpClient = new SmtpClient
            {
                Host = _smtpSettings.Host,
                Port = _smtpSettings.Port,
                EnableSsl = _smtpSettings.EnableSsl,
                Credentials = new NetworkCredential(_smtpSettings.Username, _smtpSettings.Password)
            };
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
			string smtpHost = _smtpSettings.Host;
            int smtpPort = _smtpSettings.Port;
            string smtpUsername = _smtpSettings.Username;
            string smtpPassword = _smtpSettings.Password;
            bool enableSsl = _smtpSettings.EnableSsl;

            using (var client = new SmtpClient(smtpHost, smtpPort))
            {
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(smtpUsername, smtpPassword);
                client.EnableSsl = enableSsl;

                var message = new MailMessage
                {
                    From = new MailAddress(smtpUsername),
                    Subject = subject,
                    Body = htmlMessage,
                    IsBodyHtml = true
                };

                message.To.Add(email);

                await client.SendMailAsync(message);
            }
        }

        public async Task<string> SendVerificationCode(string recipientEmail)
        {
            string verificationCode = _random.Next(10000, 99999).ToString();
            MailMessage mailMessage = new MailMessage(_smtpSettings.Username, recipientEmail)
            {
                Subject = "Email Verification Code",
                Body = $"Your verification code is: {verificationCode}"
            };

            _smtpClient.SendMailAsync(mailMessage);

            return verificationCode;
        }
    }
}
