using RoeiVereniging.Core.Data.Helpers;
using System;
using System.Net;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;

namespace RoeiVereniging.Core.Services
{
    public class BadweatherCheckerBackgroundServices
    {
        private CancellationTokenSource _cts;
        private MailHelper _mailHelper;

        public void Start()
        {
            _mailHelper = new MailHelper();
            _cts = new CancellationTokenSource();
            Task.Run(() => PingLoop(_cts.Token));
        }

        public void Stop()
        {
            _cts?.Cancel();
        }

        // while app is running, send a mail every hour
        private async Task PingLoop(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                _mailHelper.SendDangerousWeatherMail("11-12-2025", "17:00", "Domme Dolfijn", "test@mail.addr");
                await Task.Delay(TimeSpan.FromSeconds(5), token);
            }
        }
    }
}