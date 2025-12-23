using Microsoft.Maui.Controls;
using RoeiVereniging.ViewModels;


namespace RoeiVereniging.Views;

public partial class BoatListView : ContentPage
{
	public BoatListView()
	{
		InitializeComponent();
		BindingContext = new BoatListViewModel();
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is BoatListViewModel vm)
        {
            vm.Refresh();
        }
    }

    private async void OnBotenClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(BoatListView), true);
    }

    private void OnGebruikersClicked(object sender, EventArgs e)
    {
        // TODO: Navigatie naar gebruikerspagina
    }

    private void OnReparatiesClicked(object sender, EventArgs e)
    {
        // TODO: Navigatie naar reparatiespagina
    }

    private void OnOverzichtClicked(object sender, EventArgs e)
    {
        // TODO: Navigatie naar overzichtspagina
    }
}