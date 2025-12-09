using RoeiVereniging.Core.Models;
using RoeiVereniging.ViewModels;

namespace RoeiVereniging.Views;

public partial class ReserveBoatView : ContentPage
{
    private readonly ReserveBoatViewModel _viewModel;
    public ReserveBoatView(ReserveBoatViewModel viewModel)
	{
		InitializeComponent();

		BindingContext = viewModel;
        _viewModel = viewModel;
    }

    private void AttributeChanged(object sender, EventArgs e)
    {
        if (_viewModel == null) return;
        _viewModel.AttributeChanged();
    }
}