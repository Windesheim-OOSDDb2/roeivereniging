using CommunityToolkit.Mvvm.ComponentModel;
using RoeiVereniging.Core.Data.Helpers;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Maui.Graphics;
using RoeiVereniging.Core.Models;
using RoeiVereniging.Core.Helpers;
using RoeiVereniging.Core.Interfaces.Services;

namespace RoeiVereniging.ViewModels
{
    public partial class WeatherViewModel : BaseViewModel
    {
        private string apiKey = "a3ed4a6567";
        private readonly IReservationService _reservationService;
        private readonly IUserService _userService;

        MailHelper mailHelper = new();

        [ObservableProperty]
        public LiveWeerV2? liveWeather = null;

        [ObservableProperty]
        public WkVerwUi[] wkVerw = Array.Empty<WkVerwUi>();

        [ObservableProperty]
        private Color liveBackgroundColor = Colors.White;
        
        [ObservableProperty]
        public string liveWeatherIcon = string.Empty;

        [ObservableProperty]
        public string liveWeatherMessage = string.Empty;

        // Settings for weather evaluation
        public double ColdThreshold { get; set; } = 10.0;
        public int MaxWindBft = 9;
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

        public WeatherViewModel(IReservationService reservationService, IUserService userService)
        {
            _reservationService = reservationService;
            _userService = userService;
            _ = InitializeAsync();
        }

        private async Task InitializeAsync()
        {
            var live = await WeatherHelper.GetWeatherAsync("zwolle", apiKey);
            if (live != null)
            {
                LiveWeather = live.LiveWeer != null && live.LiveWeer.Length > 0 ? live.LiveWeer[0] : null;

                WkVerw = live.WkVerw != null
                    ? live.WkVerw.Select(w => new WkVerwUi(w)).ToArray()
                    : Array.Empty<WkVerwUi>();

                UpdateBackgroundForWeather(LiveWeather);
                LiveWeatherIcon = WeatherImageFileMapperHelper.MapToImageFile(LiveWeather?.Image);
                liveWeatherMessage = string.Empty;
            }
            else
            {
                // Inform UI that weather could not be loaded
                LiveWeather = null;
                WkVerw = Array.Empty<WkVerwUi>();
                LiveWeatherIcon = string.Empty;
                LiveBackgroundColor = Color.FromRgba("#D7263D");
                LiveWeatherMessage = "Could not load weather data. Please check your network connection.";
            }

            // If API returned an empty live-weather array, show a message as well
            if (live != null && (live.LiveWeer == null || live.LiveWeer.Length == 0))
            {
                LiveWeather = null;
                LiveWeatherMessage = "No live weather data available for the selected location.";
                LiveBackgroundColor = Color.FromRgba("#D7263D");
            }
        }

        partial void OnLiveWeatherChanged(LiveWeerV2? value)
        {
            if (value is null)
                LiveWeatherMessage = "Weather data unavailable.";
            else
                LiveWeatherMessage = string.Empty;

            UpdateBackgroundForWeather(value);
            LiveWeatherIcon = WeatherImageFileMapperHelper.MapToImageFile(value?.Image);
        }

        private void UpdateBackgroundForWeather(LiveWeerV2? weather)
        {
            if (IsDangerousWeather(weather))
            {
                LiveBackgroundColor = Color.FromRgba("#D7263D");
                return;
            }

            UpdateBackgroundForTemperature(weather?.Temp);
        }

        private bool IsDangerousWeather(LiveWeerV2? weather)
        {
            if (weather is null)
                return false;

            string imageKey = weather.Image ?? string.Empty;

            foreach (var kw in dangerKeywords)
            {
                if (!imageKey.Contains(kw, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            if (weather.WindBft >= MaxWindBft)
            {
                return true;
            }

            return false;
        }

        private void UpdateBackgroundForTemperature(double? temp)
        {
            if (temp is null)
            {
                LiveBackgroundColor = Color.FromRgba("#D7263D");
                return;
            }

            LiveBackgroundColor = temp < ColdThreshold ? Color.FromRgba("#D7263D") : Color.FromRgba("#0854D1");
        }
    }
}

