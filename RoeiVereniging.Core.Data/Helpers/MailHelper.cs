using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Mail;

namespace RoeiVereniging.Core.Data.Helpers
{
    public class MailHelper
    {
        public async Task SendMail(string userEmail, string subject, string message)
        {
            var styledMessage = $"<div style=\"width: 100%; align-content: center; justify-content:center; background-color:#5FA6E8;\">\r\n <h1 style=\"text-align: center; margin: auto; width: fit-content; font-weight: bold; font-size: 5em; color: red;\">{subject}</h1>" +
                $"\r\n <p style=\"text-align: center; margin: 50px auto; padding: 1rem; border-radius: 15px; color: white; width: fit-content; font-size: 3em; font-weight:900; background-color: #082757\">{message}</p>\r\n</div>";

            using var client = new SmtpClient("sandbox.smtp.mailtrap.io", 2525)
            {
                Credentials = new NetworkCredential("d62aaaf8aa2dc2", "3033c5ff558831"),
                EnableSsl = true
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress("alarment@roeimeister.nl"),
                Subject = subject,
                Body = styledMessage,
                IsBodyHtml = true
            };

            mailMessage.To.Add(userEmail);

            await client.SendMailAsync(mailMessage);

            Console.WriteLine("Sent");
        }
    }
}
