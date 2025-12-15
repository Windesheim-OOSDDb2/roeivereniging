using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net;
using System.Net.Mail;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace RoeiVereniging.Core.Data.Helpers
{
    public class MailHelper
    {
        private string Username { get; set; }
        private string Password { get; set; }

        public MailHelper()
        {
            // Get mailserver settings from appsettings.json
            IConfigurationRoot config = new ConfigurationBuilder().SetBasePath(AppDomain.CurrentDomain.BaseDirectory).AddJsonFile("appsettings.json").Build();
            IConfigurationSection section = config.GetSection("MailServerStrings");
            Username = section.GetValue<string>("username");
            Password = section.GetValue<string>("password");
        }

        public void SendDangerousWeatherMail(DateTime date, TimeSpan time, string boatName, string recipient)
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
                        <strong>Reservering:</strong> {date} - {time}
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

            Mail(styledMessage, "Roeimeister, reservering on hold gezet", recipient);
        }

        public void Mail(string styledMessage, string subject, string recipient)
        {
            var client = new SmtpClient("sandbox.smtp.mailtrap.io", 2525)
            {
                Credentials = new NetworkCredential(Username, Password),
                EnableSsl = true
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress("alarmen@roeimeister.nl"),
                Subject = "subject",
                Body = styledMessage,
                IsBodyHtml = true
            };


            mailMessage.To.Add("recipient@test.mail");

            client.Send(mailMessage);
        }
    }
}
