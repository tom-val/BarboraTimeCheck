using System;
using System.Collections.Generic;
using System.Text;

namespace BarboraTimeCheck.Services.Models
{
    public class Settings
    {
        public int CheckInterval { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool EmailNotifications { get; set; }
        public bool PushNotifications { get; set; }
        public string EmailFrom { get; set; }
        public string EmailUsername { get; set; }
        public string EmailPassword { get; set; }
        public string DeliveryEmail { get; set; }
        public string EmailSmtpServer { get; set; }
        public string AuthCookie { get; set; }
    }
}
