using CommunityToolkit.Mvvm.ComponentModel;
using RoeiVereniging.Core.Data.Helpers;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace RoeiVereniging.ViewModels
{
    public partial class WeatherViewModel : BaseViewModel
    {
        [ObservableProperty]
        public LiveWeerV2? liveWeather = null;

        [ObservableProperty]
        public WkVerwUi[] wkVerw = Array.Empty<WkVerwUi>();

        public WeatherViewModel()
        {
            _ = InitializeAsync();
        }

        private async Task InitializeAsync()
        {
            string apiKey = "a3ed4a6567";
            var live = await WeatherHelper.GetWeatherAsync("zwolle", apiKey);
            if (live != null)
            {
                LiveWeather = live.LiveWeer != null && live.LiveWeer.Length > 0 ? live.LiveWeer[0] : null;

                WkVerw = live.WkVerw != null
                    ? live.WkVerw.Select(w => new WkVerwUi(w)).ToArray()
                    : Array.Empty<WkVerwUi>();

                Debug.WriteLine($"Weather data loaded: {LiveWeather?.Temp}");
                if (WkVerw.Length > 0)
                    Debug.WriteLine($"Forecast days: {WkVerw[0].Dag}");
            }
        }

        public class WkVerwUi
        {
            private const string IconsSubFolder = "WeatherIcons";
            private static readonly string FallbackImage = "bewolkt.png";

            private readonly WkVerw _dto;

            public WkVerwUi(WkVerw dto) => _dto = dto ?? throw new ArgumentNullException(nameof(dto));

            public string Dag => _dto.Dag ?? string.Empty;
            public double MinTemp => _dto.MinTemp;
            public double MaxTemp => _dto.MaxTemp;

            public string TempRange => $"{MinTemp:0.#} / {MaxTemp:0.#} °C";

            // The DTO's `Image` value (e.g. "zonnig", "regen", "lichtbewolkt", ...)
            public string ImageKey => _dto.Image ?? string.Empty;

            public string IconImage => MapToImageFile(ImageKey);

            private static string MapToImageFile(string key)
            {
                if (string.IsNullOrWhiteSpace(key))
                    return FallbackImage;

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

                return $"{IconsSubFolder}/{filename}";
            }
        }
    }
}

