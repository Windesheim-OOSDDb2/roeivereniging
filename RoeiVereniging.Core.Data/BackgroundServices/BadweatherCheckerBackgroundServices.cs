using RoeiVereniging.Core.Data.Helpers;
using RoeiVereniging.Core.Interfaces.Services;
using RoeiVereniging.Core.Models;
using RoeiVereniging.Core.Repositories;
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
        private readonly IReservationService _reservationService;
        private readonly IUserService _userService;
        private readonly IBoatService _boatService;
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

        public BadweatherCheckerBackgroundServices(IEmailService mailService, IReservationService reservationService, IUserService userService, IBoatService boatService)
        {
            _mailService = mailService;
            _reservationService = reservationService;
            _userService = userService;
            _boatService = boatService;
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
                    // check if temp is below 10 or imagekey contains dangerous weather since imagekey is identical to a weather description
                    if (wkVerw[i].MinTemp < 10 || dangerKeywords.Any(keyword => string.Equals(wkVerw[i].ImageKey, keyword, StringComparison.OrdinalIgnoreCase)))
                    {
                        List<Reservation> reservations = _reservationService.GetByDate(DateTime.Parse(wkVerw[i].Datum));
                        foreach (Reservation reservation in reservations.Where(r => r.Messaged == 0))
                        {
                            try
                            {
                                _mailService.SendDangerousWeatherMail(reservation.StartTime, _boatService.GetById(reservation.BoatId).name, _userService.GetById(reservation.UserId).EmailAddress);
                                _reservationService.MarkMessaged(reservation.Id);
                            }
                            catch (Exception ex)
                            {
                                Debug.WriteLine($"Error sending dangerous weather mail: {ex.Message}");
                            }
                        }
                    }
                }

                _reservationService.GetAll();

                await Task.Delay(TimeSpan.FromHours(1), token);
            }
        }
    }
}