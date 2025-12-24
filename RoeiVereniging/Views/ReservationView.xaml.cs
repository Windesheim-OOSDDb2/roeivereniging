namespace RoeiVereniging.Views;

using RoeiVereniging.Core.Data.Repositories;
using RoeiVereniging.Core.Services;
using RoeiVereniging.ViewModels;
using Microsoft.Maui.Controls.StyleSheets;
using RoeiVereniging.Core.Models;

public partial class ReservationView : ContentPage
{
    private readonly ReservationViewModel _viewModel;
    public ReservationView(ReservationViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    private async void OnReservationSelected(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is ReservationViewDTO selectedReservation)
        {
            await Shell.Current.GoToAsync(nameof(ReservationDetailView), new Dictionary<string, object>
            {
                { "Reservation", selectedReservation }
            });

            ((CollectionView)sender).SelectedItem = null;
        }
    }
} 