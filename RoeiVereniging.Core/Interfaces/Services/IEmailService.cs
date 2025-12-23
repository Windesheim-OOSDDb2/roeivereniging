namespace RoeiVereniging.Core.Interfaces.Services
{
    public interface IEmailService
    {
        void SendDangerousWeatherMail(DateTime dateTime, string boatName, string recipient);
        void SendMail(string htmlBody, string subject, string recipient);
    }
}