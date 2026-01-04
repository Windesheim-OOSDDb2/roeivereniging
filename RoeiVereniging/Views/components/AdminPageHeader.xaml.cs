using CommunityToolkit.Mvvm.Input;
using RoeiVereniging.ViewModels;
using System.Threading.Tasks;

namespace RoeiVereniging.Views.Components;

public partial class AdminPageHeader : ContentView
{
    private readonly GlobalViewModel _global = new();
    public AdminPageHeader()
	{
		InitializeComponent();
    }

    private async void OnBotenClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(BoatListView), true);
    }

    private async void OnGebruikersClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(UserView), true);
    }

    private void OnReparatiesClicked(object sender, EventArgs e)
    {
        // TODO: Navigatie naar reparatiespagina
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