namespace RoeiVereniging.Core.Interfaces.Services
{
    public interface IEmailService
    {
        void SendDangerousWeatherMail(string date, string time, string boatName, string recipient);
        void SendMail(string htmlBody, string subject, string recipient);
    }
}