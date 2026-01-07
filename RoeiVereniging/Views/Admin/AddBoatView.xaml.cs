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


}