using Microsoft.Extensions.Configuration;
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
        private readonly string _apiKey;
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

            // Get api key from environment variable
            IConfigurationRoot config = new ConfigurationBuilder().SetBasePath(AppDomain.CurrentDomain.BaseDirectory).AddJsonFile("appsettings.json").Build(); ;
            IConfigurationSection section = config.GetSection("WeatherApiStrings");
            _apiKey = section.GetValue<string>("WeatherCheckerApiKey");
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

        // while app is running, check for bad weather every hour and if badweather is expected in the next 3 days, send email to users with reservations on those days
        private async Task PingLoop(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                var Response = await WeatherHelper.GetWeatherAsync("zwolle", _apiKey);

                wkVerw = Response?.WkVerw != null
                    ? Response.WkVerw.Select(w => new WkVerwUi(w)).ToArray()
                    : Array.Empty<WkVerwUi>();

                if (Response == null)
                {
                    Debug.WriteLine("Error: Weather API response is null.");
                }

                
                foreach (var weather in wkVerw.Take(3))
                {
                    if (weather.MinTemp < 10 || dangerKeywords.Any(keyword => string.Equals(weather.ImageKey, keyword, StringComparison.OrdinalIgnoreCase)))
                    {
                        var reservations = _reservationService.GetByDate(DateTime.Parse(weather.Datum))
                            .Where(r => r.Messaged == 0);

                        foreach (var reservation in reservations)
                        {
                            try
                            {
                                var boat = _boatService.GetById(reservation.BoatId);
                                var user = _userService.Get(reservation.UserId);

                                if (boat != null && user != null)
                                {
                                    _mailService.SendDangerousWeatherMail(reservation.StartTime, boat.name, user.EmailAddress);
                                    reservation.Messaged = 1;
                                    _reservationService.MarkMessaged(reservation.Id);
                                }
                            }
                            catch (Exception ex)
                            {
                                Debug.WriteLine($"Error sending dangerous weather mail: {ex.Message}");
                            }
                        }
                    }
                }
                // check if array is not empty before looping  
                if (wkVerw.Length > 0)
                { 
                    for (int i = 0; i < Math.Min(3, wkVerw.Length); i++)
                    {
                        if (wkVerw[i].MinTemp < 10 || dangerKeywords.Any(keyword => string.Equals(wkVerw[i].ImageKey, keyword, StringComparison.OrdinalIgnoreCase)))
                        {
                            List<Reservation> reservations = _reservationService.GetByDate(DateTime.Parse(wkVerw[i].Datum));
                            foreach (Reservation reservation in reservations.Where(r => r.Messaged == 0))
                            {
                                try
                                {
                                    _mailService.SendDangerousWeatherMail(reservation.StartTime, _boatService.GetById(reservation.BoatId).name, _userService.Get(reservation.UserId).EmailAddress);
                                    _reservationService.MarkMessaged(reservation.Id);
                                }
                                catch (Exception ex)
                                {
                                    Debug.WriteLine($"Error sending dangerous weather mail: {ex.Message}");
                                }
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