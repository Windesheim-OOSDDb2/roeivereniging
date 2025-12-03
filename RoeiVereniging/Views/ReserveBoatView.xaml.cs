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

    private void TypePicker_SelectedIndexChanged(object sender, EventArgs e)
    {
        var picker = (Picker)sender;
        int selectedIndex = picker.SelectedIndex;

        if (selectedIndex != -1)
        {
            BoatType Type = (BoatType)picker.SelectedItem;
            _viewModel.NewSelectedBoat(Type);
        }
    }
}