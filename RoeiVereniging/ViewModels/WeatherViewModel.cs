using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RoeiVereniging.Core.Data.Helpers;
using System.Diagnostics;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RoeiVereniging.ViewModels
{
    public partial class WeatherViewModel : BaseViewModel
    {

        [ObservableProperty]
        public LiveWeerV2? liveWeather = null;

        [ObservableProperty]
        public WkVerw[] wkVerw = Array.Empty<WkVerw>();

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
                WkVerw = live.WkVerw ?? Array.Empty<WkVerw>();

                Debug.WriteLine($"Weather data loaded: {LiveWeather.Temp}");
                Debug.WriteLine($"Forecast days: {WkVerw[0].Dag}");
            }
        }
    }
}

