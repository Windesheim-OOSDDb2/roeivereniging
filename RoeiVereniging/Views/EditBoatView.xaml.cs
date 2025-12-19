using RoeiVereniging.ViewModels;
namespace RoeiVereniging.Views;


public partial class EditBoatView : ContentPage
{
	public EditBoatView(EditBoatViewModel viewmodel)
	{
		InitializeComponent();
        BindingContext = viewmodel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        (BindingContext as EditBoatViewModel)?.OnAppearing();
    }

}