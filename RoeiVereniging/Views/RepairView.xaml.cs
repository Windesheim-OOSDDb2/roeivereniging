namespace RoeiVereniging.Views;

public partial class RepairView : ContentPage
{
	public RepairView()
	{
		InitializeComponent();
		BindingContext = new ViewModels.RepairViewModel();
    }
}