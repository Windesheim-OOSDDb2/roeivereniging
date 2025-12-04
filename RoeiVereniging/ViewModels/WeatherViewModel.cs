using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RoeiVereniging.Core.Data.Helpers;
using System.Threading.Tasks;

namespace RoeiVereniging.ViewModels
{
    public partial class WeatherViewModel : BaseViewModel
    {
        public WeatherViewModel()
        {
            _ = InitializeAsync();
        }

        private async Task InitializeAsync()
        {
            string apiKey = "a3ed4a6567";
            var live = await WeatherHelper.GetWeatherAsync("Zwolle", apiKey);
            if (live != null)
            {
                var temp = live.Temp;
                var summary = live.Samenv;
                // update UI / viewmodel...

                // Show in debug output
                System.Diagnostics.Debug.WriteLine($"Current temperature in {live.Plaats}: {temp}°C, {summary}");
            }
        }
    }
}

