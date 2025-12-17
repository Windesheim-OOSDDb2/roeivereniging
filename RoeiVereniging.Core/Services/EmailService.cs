using Microsoft.Extensions.Configuration;
using RoeiVereniging.Core.Interfaces.Services;
using System.Net;
using System.Net.Mail;

namespace RoeiVereniging.Core.Services
{
    public class EmailService : IEmailService
    {
        private readonly string _username;
        private readonly string _password;
        private readonly string _host;
        private readonly int _port;

        public EmailService()
        {
            IConfigurationRoot config = new ConfigurationBuilder().SetBasePath(AppDomain.CurrentDomain.BaseDirectory).AddJsonFile("appsettings.json").Build();;
            IConfigurationSection section = config.GetSection("MailServerStrings");
            _username = section.GetValue<string>("Username");
            _password = section.GetValue<string>("Password");
            _host = section.GetValue<string>("Host");
            _port = section.GetValue<int>("Port");
        }

        public void SendDangerousWeatherMail(DateTime dateTime, string boatName, string recipient)
        {
            string styledMessage = $@"
                <table width=""100%"" cellspacing=""0"" cellpadding=""0"" style=""max-width:600px; margin:auto; background:#ffffff; border-radius:12px; padding:0;"">
                  <tr>
                    <td style=""padding:20px; text-align:center;"">
                      <img src=""https://raw.githubusercontent.com/Windesheim-OOSDDb2/roeivereniging/refs/heads/master/RoeiVereniging/Resources/Images/logo.png"" alt=""RoeiMeister"" style=""max-width:120px; margin-bottom:10px;"" />
                      <h2 style=""margin:0; color:#0A2A52; font-family:Arial, sans-serif;"">RoeiMeister Update</h2>
                    </td>
                  </tr>

                  <tr>
                    <td style=""background:#E7ECFB; padding:25px; border-radius:12px; font-family:Arial, sans-serif;"">
                      <h3 style=""margin:0 0 10px 0; color:#0A2A52;"">Weer waarschuwing</h3>
                      <p style=""margin:0; font-size:15px; color:#0A2A52;"">
                        <strong>Reservering:</strong> {dateTime.ToShortDateString()} - {dateTime.Hour} : {dateTime.Minute}
                      </p>
                      <p style=""margin:5px 0 15px; font-size:15px; color:#0A2A52;"">
                        <strong>Boot:</strong> {boatName}
                      </p>

                      <p style=""font-size:15px; line-height:1.5; margin:0; color:#0A2A52;"">
                        Er is slecht weer voorspeld voor de datum en tijd van je reservering. Om je veiligheid te waarborgen, hebben we je reservering tijdelijk on hold gezet. We raden je aan om de weersvoorspellingen in de gaten te houden en contact met ons op te nemen als je vragen hebt.
                      </p>

                      <div style=""margin-top:20px; padding:12px; background:#0066CC; color:#ffffff; text-align:center; border-radius:8px; font-size:15px;"">
                        Je reservering staat tijdelijk on hold vanwege voorspeld slecht weer.
                      </div>
                    </td>
                  </tr>

                  <tr>
                    <td style=""text-align:center; padding:20px; font-size:13px; color:#0A2A52; font-family:Arial, sans-serif;"">
                      Blijf veilig en bedankt dat je voor RoeiMeister kiest!
                    </td>
                  </tr>
                </table>
                ";
            SendMail(styledMessage, "Roeimeister, reservering on hold gezet", recipient);
        }

        public void SendMail(string htmlBody, string subject, string recipient)
        {
            using var client = new SmtpClient(_host, _port)
            {
                Credentials = new NetworkCredential(_username, _password),
                EnableSsl = true
            };

            using var mailMessage = new MailMessage
            {
                From = new MailAddress("alarmen@roeimeister.nl"),
                Subject = subject,
                Body = htmlBody,
                IsBodyHtml = true
            };

            mailMessage.To.Add(recipient);

            client.Send(mailMessage);
        }
    }
}