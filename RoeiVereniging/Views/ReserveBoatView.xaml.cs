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

    private void Picker_SelectedIndexChanged(object sender, EventArgs e)
    {
        var picker = (Picker)sender;
        int selectedIndex = picker.SelectedIndex;

        if (selectedIndex != -1)
        {
            Boat product = picker.SelectedItem as Boat;
            _viewModel.NewSelectedBoat(product);
        }
    }
}