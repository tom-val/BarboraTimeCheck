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

            mail.From = new MailAddress("barboratest123@gmail.com");
            mail.To.Add(ConfigurationManager.AppSettings["deliveryEmail"]);
            mail.Subject = subject;
            mail.Body = text;

            SmtpServer.Port = 587;
            SmtpServer.UseDefaultCredentials = false;
            SmtpServer.Credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["emailUsername"], ConfigurationManager.AppSettings["emailPassword"]);
            SmtpServer.EnableSsl = true;
            SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;

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
