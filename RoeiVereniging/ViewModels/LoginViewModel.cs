using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RoeiVereniging;
using RoeiVereniging.Core.Interfaces.Services;
using RoeiVereniging.Core.Models;
using RoeiVereniging.ViewModels;
using RoeiVereniging.Views;
using RoeiVereniging.Views.Admin;

namespace RoeiVereniging.ViewModels;
public partial class LoginViewModel : BaseViewModel
{
    private readonly GlobalViewModel _global;
    private readonly IAuthService _authService;

    [ObservableProperty] private string email;
    [ObservableProperty] private string password;
    [ObservableProperty] private string loginMessage;

    public LoginViewModel(IAuthService authService, GlobalViewModel global)
    {
        _authService = authService;
        _global = global;
    }

    [RelayCommand]
    private async void Login()
    {
        User? user = _authService.Login(email, password);
        if (user != null)
        {
            _global.currentUser = user;
        }
        else
        {
            LoginMessage = "Ongeldige inloggegevens.";
            return;
        }

        if (_authService.IsAdmin(user))
        {
            await Shell.Current.GoToAsync(nameof(AdminDashboardView));
        }
        else
        {
            await Shell.Current.GoToAsync(nameof(ReserveBoatView));
        }
    }
}
