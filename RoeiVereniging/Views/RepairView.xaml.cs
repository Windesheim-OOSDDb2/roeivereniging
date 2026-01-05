using RoeiVereniging.ViewModels;
namespace RoeiVereniging.Views;

public partial class RepairView : ContentPage
{
	public RepairView(RepairViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        (BindingContext as RepairViewModel)?.LoadRepairs();
    }

}