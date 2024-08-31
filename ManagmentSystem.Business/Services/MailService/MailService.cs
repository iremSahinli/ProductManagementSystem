using MailKit.Security;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagmentSystem.Business.Services.MailService
{
    public class MailService : IMailService
    {

        private readonly MailSettings _mailSettings;

        public MailService(IOptions<MailSettings> mailSettings)
        {
            _mailSettings = mailSettings.Value;
        }
        public async Task SendMailAsync(string email, string subject, string message)
        {
            try
            {
                var newEmail = new MimeMessage();
                newEmail.From.Add(MailboxAddress.Parse(_mailSettings.SenderEmail));
                newEmail.To.Add(MailboxAddress.Parse(email));
                newEmail.Subject = subject;
                var builder = new BodyBuilder
                {
                    HtmlBody = message
                };
                newEmail.Body = builder.ToMessageBody();
                using var smtp = new SmtpClient();
                await smtp.ConnectAsync(_mailSettings.SmtpServer, _mailSettings.Port, SecureSocketOptions.StartTls);
                await smtp.AuthenticateAsync(_mailSettings.SenderEmail, _mailSettings.Password);
                await smtp.SendAsync(newEmail);
                await smtp.DisconnectAsync(true);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"E-posta gönderilirken bir hata oluştu: {ex.Message}");
            }
        }
    }
}
