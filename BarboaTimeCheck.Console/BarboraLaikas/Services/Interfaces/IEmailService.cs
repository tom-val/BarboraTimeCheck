namespace BarboraLaikas.Services
{
    public interface IEmailService
    {
        void CheckEmailConfiguration();
        void SendEmail(string subject, string text);
    }
}