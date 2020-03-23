using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;

namespace BarboraTimeCheck.Services
{
    public class EmailService
    {
        private SettingsService settingsService;
        public EmailService()
        {
            settingsService = new SettingsService();
        }

        public void SendEmail(string subject, string text)
        {
            var settings = settingsService.GetSettings();

            MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient(settings.EmailSmtpServer);

            mail.From = new MailAddress(settings.EmailUsername);
            mail.To.Add(settings.DeliveryEmail);
            mail.Subject = subject;
            mail.Body = text;

            SmtpServer.Port = 587;
            SmtpServer.Credentials = new System.Net.NetworkCredential(settings.EmailUsername, settings.EmailPassword);
            SmtpServer.EnableSsl = true;

            SmtpServer.Send(mail);
        }
    }
}
