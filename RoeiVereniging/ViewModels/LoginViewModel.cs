using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RoeiVereniging;
using RoeiVereniging.Core.Interfaces.Services;
using RoeiVereniging.ViewModels;

namespace RoeiVereniging.ViewModels;
public partial class LoginViewModel : BaseViewModel
{
    private readonly IAuthService _authService;

    [ObservableProperty] private string email = "test@test.nl";
    [ObservableProperty] private string password = "testpass";
    [ObservableProperty] private string loginMessage;

    public LoginViewModel(IAuthService authService)
    {
        _authService = authService;
    }

    [RelayCommand]
    private void Login()
    {
        var user = _authService.Login(email, password);
        if (user != null)
        {
            LoginMessage = $"welkom {user.Name}!";
            // navigate to AppShell or main page:
            Application.Current.MainPage = new AppShell();
        }
        else
        {
            LoginMessage = "Ongeldige inloggegevens.";
        }
    }
}
