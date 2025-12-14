using RoeiVereniging.Core.Data.Helpers;
using RoeiVereniging.Core.Interfaces.Services;
using RoeiVereniging.Core.Models;
using System;
using System.Diagnostics;
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
        private string apiKey = "beb9920c3e";
        public WkVerwUi[] wkVerw = Array.Empty<WkVerwUi>();
        public string[] dangerKeywords = new[]
            {
                "bliksem",      // lightning (EN)
                "hagel",        // hail (EN)
                "mist",         // Fog (EN)
                "sneeuw",       // Snow (EN)
                "nachtmist",    // Night Fog (EN)
                "helderenacht", // Clear night (EN)
                "nachtbewolkt"  // Cloudy at night (EN)
            };

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
                var Response = await WeatherHelper.GetWeatherAsync("zwolle", apiKey);

                wkVerw = Response.WkVerw != null
                    ? Response.WkVerw.Select(w => new WkVerwUi(w)).ToArray()
                    : Array.Empty<WkVerwUi>();

                

                // Check the first 3 entries of wkVerw for min temp < 10 or dangerous weather image
                for (int i = 0; i < Math.Min(3, wkVerw.Length); i++)
                {
                    if (wkVerw[i].MinTemp < 10 || dangerKeywords.Any(keyword => string.Equals(wkVerw[i].ImageKey, keyword, StringComparison.OrdinalIgnoreCase)))
                    {
                        Debug.WriteLine("Bad weather detected, sending email...");
                    }
                }

                await Task.Delay(TimeSpan.FromHours(1), token);
            }
        }
    }
}