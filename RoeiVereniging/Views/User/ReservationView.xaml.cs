namespace RoeiVereniging.Views;

using RoeiVereniging.Core.Data.Repositories;
using RoeiVereniging.Core.Services;
using RoeiVereniging.ViewModels;
using Microsoft.Maui.Controls.StyleSheets;
using RoeiVereniging.Core.Models;

public partial class ReservationView : ContentPage
{
    public ReservationView(ReservationViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
} 