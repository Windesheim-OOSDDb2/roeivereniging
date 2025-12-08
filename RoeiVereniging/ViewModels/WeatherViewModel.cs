using CommunityToolkit.Mvvm.ComponentModel;
using RoeiVereniging.Core.Data.Helpers;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Maui.Graphics;
using System.Globalization;

namespace RoeiVereniging.ViewModels
{
    public partial class WeatherViewModel : BaseViewModel
    {
        private string apiKey = "a3ed4a6567";

        [ObservableProperty]
        public LiveWeerV2? liveWeather = null;

        [ObservableProperty]
        public WkVerwUi[] wkVerw = Array.Empty<WkVerwUi>();

        [ObservableProperty]
        private Color liveBackgroundColor = Colors.White;
        
        [ObservableProperty]
        public string liveWeatherIcon = string.Empty;

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

        public WeatherViewModel()
        {
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
                LiveWeatherIcon = MapToImageFile(LiveWeather?.Image);

                Debug.WriteLine($"Weather data loaded: {LiveWeather?.Temp}");
                if (WkVerw.Length > 0)
                    Debug.WriteLine($"Forecast days: {WkVerw[0].Dag}");
            }
        }

        partial void OnLiveWeatherChanged(LiveWeerV2? value)
        {
            UpdateBackgroundForWeather(value);
            LiveWeatherIcon = MapToImageFile(value?.Image);
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
                if (imageKey.Contains(kw, StringComparison.OrdinalIgnoreCase))
                    return true;
            }

            if (weather.WindBft >= MaxWindBft)
                return true;

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

        private static string MapToImageFile(string? key)
        {
            if (string.IsNullOrWhiteSpace(key))
                return $"bewolkt.png";

            var k = key.Trim().ToLowerInvariant();
            string filename = k switch
            {
                "zonnig" => "zonnig.png",
                "bliksem" => "bliksem.png",
                "regen" => "regen.png",
                "buien" => "buien.png",
                "hagel" => "hagel.png",
                "mist" => "mist.png",
                "sneeuw" => "sneeuw.png",
                "bewolkt" => "bewolkt.png",
                "lichtbewolkt" => "lichtbewolkt.png",
                "halfbewolkt" => "halfbewolkt.png",
                "halfbewolkt_regen" => "halfbewolkt_regen.png",
                "zwaarbewolkt" => "zwaarbewolkt.png",
                "nachtmist" => "nachtmist.png",
                "helderenacht" => "helderenacht.png",
                "nachtbewolkt" => "nachtbewolkt.png",
                _ => $"{k}.png"
            };

            return $"{filename}";
        }

        public class WkVerwUi
        {
            private static readonly string FallbackImage = "bewolkt.png";

            private readonly WkVerw _dto;

            public WkVerwUi(WkVerw dto) => _dto = dto ?? throw new ArgumentNullException(nameof(dto));

            public string Dag => _dto.Dag ?? string.Empty;
            public double MinTemp => _dto.MinTemp;
            public double MaxTemp => _dto.MaxTemp;

            public string TempRange => $"{MinTemp:0.#} / {MaxTemp:0.#} °C";

            public string ImageKey => _dto.Image ?? string.Empty;

            public string IconImage => MapToImageFile(ImageKey);
        }
    }
}

