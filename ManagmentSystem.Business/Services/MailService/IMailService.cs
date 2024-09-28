using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagmentSystem.Business.Services.MailService
{
    public interface IMailService
    {
        /// <summary>
        /// Belirtilen e-posta adresine e-posta gönderir.
        /// </summary>
        /// <param name="email">E-postanın gönderileceği alıcının e-posta adresi.</param>
        /// <param name="subject">E-postanın konusu.</param>
        /// <param name="message">E-postanın içeriği.</param>
        /// <returns>
        /// E-postanın başarıyla gönderilip gönderilmediğini belirten asenkron bir görev.
        /// </returns>
        Task SendMailAsync(string email, string subject, string message);
    }
}
