namespace RoeiVereniging.Views;

using RoeiVereniging.Core.Data.Repositories;
using RoeiVereniging.Core.Services;
using RoeiVereniging.ViewModels;
using Microsoft.Maui.Controls.StyleSheets;

public partial class ReservationView : ContentPage
{
    private readonly ReservationViewModel _viewModel;
    public ReservationView(ReservationViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}