using RoeiVereniging.Core.Data.Helpers;
using RoeiVereniging.Core.Interfaces.Services;
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
        private IEmailService _mailService;

        public BadweatherCheckerBackgroundServices(IEmailService mailService)
        {
            _mailService = mailService;
        }

        public void Start()
        {
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
                _mailService.SendDangerousWeatherMail("11-12-2025", "17:00", "Domme Dolfijn", "test@mail.addr");
                await Task.Delay(TimeSpan.FromSeconds(5), token);
            }
        }
    }
}