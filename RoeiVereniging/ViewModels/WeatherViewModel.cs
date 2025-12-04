using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RoeiVereniging.Core.Data.Helpers;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RoeiVereniging.ViewModels
{
    public partial class WeatherViewModel : BaseViewModel
    {
        [ObservableProperty]
        public string plaats = string.Empty;

        [ObservableProperty]
        public double temp = 0.0;

        [ObservableProperty]
        public double gtemp = 0.0;

        [ObservableProperty]
        public string samenv = string.Empty;

        [ObservableProperty]
        public int lv = 0;

        [ObservableProperty]
        public string windR = string.Empty;

        [ObservableProperty]
        public double windKmH = 0.0;

        [ObservableProperty]
        public int windBft = 0;

        [ObservableProperty]
        public string sup = string.Empty;

        [ObservableProperty]
        public string sunder = string.Empty;

        [ObservableProperty]
        public string image = string.Empty;

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
                // Update ViewModel properties
                Plaats = live.Plaats;
                Temp = live.Temp;
                Gtemp = live.GTemp;
                Samenv = live.Samenv;
                Lv = live.LV;
                WindR = live.WindR;
                WindKmH = live.WindKmH;
                WindBft = live.WindBft;
                Sup = live.Sup;
                Sunder = live.Sunder;
                Image = live.Image;
            }
        }
    }
}

