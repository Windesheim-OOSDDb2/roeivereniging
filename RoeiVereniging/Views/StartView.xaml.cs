using RoeiVereniging.ViewModels;

namespace RoeiVereniging.Views;

public partial class StartView : ContentPage
{
	public StartView(StartViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
    }
}