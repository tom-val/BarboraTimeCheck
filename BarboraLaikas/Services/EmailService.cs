using System;
using System.Configuration;
using System.Net.Mail;

namespace BarboraLaikas.Services
{
    public class EmailService : IEmailService
    {
        public void SendEmail(string subject, string text)
        {
            CheckEmailConfiguration();

            MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient(ConfigurationManager.AppSettings["emailSmtp"]);

            mail.From = new MailAddress(ConfigurationManager.AppSettings["emailUsername"]);
            mail.To.Add(ConfigurationManager.AppSettings["deliveryEmail"]);
            mail.Subject = subject;
            mail.Body = text;

            SmtpServer.Port = 587;
            SmtpServer.Credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["emailUsername"], ConfigurationManager.AppSettings["emailPassword"]);
            SmtpServer.EnableSsl = true;

            SmtpServer.Send(mail);
        }

        public void CheckEmailConfiguration()
        {
            if(string.IsNullOrEmpty(ConfigurationManager.AppSettings["emailUsername"])
                || string.IsNullOrEmpty(ConfigurationManager.AppSettings["emailUsername"])
                || string.IsNullOrEmpty(ConfigurationManager.AppSettings["emailUsername"])
                || string.IsNullOrEmpty(ConfigurationManager.AppSettings["emailUsername"]))
            {
                throw new Exception("Email configuration is invalid");
            }
        }
    }
}
