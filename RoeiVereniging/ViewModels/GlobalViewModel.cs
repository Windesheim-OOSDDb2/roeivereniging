using CommunityToolkit.Mvvm.ComponentModel;
using RoeiVereniging.Core.Models;
using RoeiVereniging.Views;

namespace RoeiVereniging.ViewModels
{
    public partial class GlobalViewModel : BaseViewModel
    {
        public User? currentUser { get; set; }

        private string currentRoute;
        public string CurrentRoute
        {
            get => currentRoute;
            set
            {
                if (SetProperty(ref currentRoute, value))
                {
                    Console.WriteLine($"CurrentRoute changed: {value}");
                    OnPropertyChanged(nameof(IsGebruikersActive));
                    OnPropertyChanged(nameof(IsBotenActive));
                    OnPropertyChanged(nameof(IsReparatiesActive));
                    OnPropertyChanged(nameof(IsOverzichtActive));
                }
            }
        }

        public bool IsGebruikersActive => CurrentRoute == nameof(UserView);
        public bool IsBotenActive => CurrentRoute == nameof(BoatListView);
        public bool IsReparatiesActive => CurrentRoute == nameof(RepairView); 
        public bool IsOverzichtActive => CurrentRoute == "OverzichtView";


        public async Task Logout()
        {
            currentUser = null;
            await Shell.Current.GoToAsync("//LoginView");
        }
    }
}
