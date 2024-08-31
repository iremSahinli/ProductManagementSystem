﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagmentSystem.Business.Services.MailService
{
    public class MailSettings
    {
        public string SmtpServer { get; set; }
        public int Port { get; set; }
        public string SenderEmail { get; set; }
        public string SenderName { get; set; }
        public string Password { get; set; }
    }
}
