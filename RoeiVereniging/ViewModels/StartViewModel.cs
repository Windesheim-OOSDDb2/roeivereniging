using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace RoeiVereniging.ViewModels
{
    public partial class StartViewModel : BaseViewModel
    {
        [ObservableProperty]
        private string welcomeMessage = "sup";

        public StartViewModel()
        {
            WelcomeMessage = "Welkom bij de Roeivereniging!";
        }

        [RelayCommand]
        private void ChangeWelcomeMessage()
        {
            WelcomeMessage = "Je hebt op de knop gedrukt!";
        }
    }
}
