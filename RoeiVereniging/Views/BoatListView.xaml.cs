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
}