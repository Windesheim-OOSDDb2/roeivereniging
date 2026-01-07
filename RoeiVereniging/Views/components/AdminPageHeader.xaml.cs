using CommunityToolkit.Mvvm.Input;
using RoeiVereniging.ViewModels;
using System.Threading.Tasks;

namespace RoeiVereniging.Views.Components;

public partial class AdminPageHeader : ContentView
{
    private GlobalViewModel _global => ((App)Application.Current).GlobalViewModel;
    public AdminPageHeader()
	{
		InitializeComponent();
        BindingContext = _global;
    }

    private async void OnBotenClicked(object sender, EventArgs e)
    {
        _global.CurrentRoute = nameof(BoatListView);
        await Shell.Current.GoToAsync(nameof(BoatListView), true);
    }

    private async void OnGebruikersClicked(object sender, EventArgs e)
    {
        _global.CurrentRoute = nameof(UserView);
        await Shell.Current.GoToAsync(nameof(UserView), true);
    }

    private async void OnReparatiesClicked(object sender, EventArgs e)
    {
        _global.CurrentRoute = nameof(RepairView);
        await Shell.Current.GoToAsync(nameof(RepairView), true);
    }

    private void OnOverzichtClicked(object sender, EventArgs e)
    {
        // TODO: Navigatie naar overzichtspagina
    }


    private async void OnLogoutClicked(object sender, EventArgs e)
    {
        await _global.Logout();
    }

}