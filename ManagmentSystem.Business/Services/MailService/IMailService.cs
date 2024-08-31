using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagmentSystem.Business.Services.MailService
{
    public interface IMailService
    {
        Task SendMailAsync(string email, string subject, string message);
    }
}
