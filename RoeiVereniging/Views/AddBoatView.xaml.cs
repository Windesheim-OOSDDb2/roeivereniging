using RoeiVereniging.Core.Models;
using RoeiVereniging.ViewModels;

namespace RoeiVereniging.Views;

public partial class AddBoatView : ContentPage
{

    private readonly AddBoatViewModel _viewModel;
    public AddBoatView(AddBoatViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
        _viewModel = viewModel;
    }

    private void SteeringWheelCheck(object sender, EventArgs e)
    {
        if (_viewModel == null) return;
        _viewModel.Test();
    }
}