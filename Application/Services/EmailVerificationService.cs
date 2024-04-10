using Application.Models;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class EmailVerificationService : IEmailVerificationService
    {
        private readonly SmtpClient _smtpClient;
        private readonly SmtpSettings _smtpSettings;
        private readonly Random _random;
        private readonly UserManager<IdentityUser> _userManager;

        public EmailVerificationService(IOptions<SmtpSettings> smtpSettings, UserManager<IdentityUser> userManager)
        {
            _smtpSettings = smtpSettings.Value;
            _smtpClient = new SmtpClient
            {
                Host = _smtpSettings.Host,
                Port = _smtpSettings.Port,
                EnableSsl = _smtpSettings.EnableSsl,
                Credentials = new NetworkCredential(_smtpSettings.Username, _smtpSettings.Password)
            };
            _random = new Random();
            _userManager = userManager;
        }

        public async Task<string> SendVerificationCode(string recipientEmail)
        {
            string verificationCode = _random.Next(10000, 99999).ToString();

            var user = await _userManager.FindByEmailAsync(recipientEmail);
            if (user == null)
            {
                user = new IdentityUser { UserName = recipientEmail, Email = recipientEmail };
                var result = await _userManager.CreateAsync(user);
                if (!result.Succeeded)
                {
                    return null;
                }
            }

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            await _userManager.ConfirmEmailAsync(user, token);

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
