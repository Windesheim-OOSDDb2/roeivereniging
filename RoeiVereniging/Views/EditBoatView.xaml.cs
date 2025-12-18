using RoeiVereniging.ViewModels;
namespace RoeiVereniging.Views;


public partial class EditBoatView : ContentPage
{
	public EditBoatView(EditBoatViewModel vm)
	{
		InitializeComponent();
        BindingContext = vm;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        (BindingContext as EditBoatViewModel)?.OnAppearing();
    }

}