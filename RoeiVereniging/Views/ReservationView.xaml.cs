namespace RoeiVereniging.Views;

using RoeiVereniging.Core.Data.Repositories;
using RoeiVereniging.Core.Services;
using RoeiVereniging.ViewModels;
using Microsoft.Maui.Controls.StyleSheets;

public partial class ReservationView : ContentPage
{
	public ReservationView()
	{
		InitializeComponent();
        BindingContext = new ReservationViewModel(new ReservationService(new ReservationRepository()));
    }
}