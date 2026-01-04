using Microsoft.Maui.Controls;
using RoeiVereniging.ViewModels;
using RoeiVereniging.Views.Admin;


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
        ((App)Application.Current).GlobalViewModel.CurrentRoute = nameof(BoatListView);

        if (BindingContext is BoatListViewModel vm)
        {
            vm.Refresh();
        }
    }
}