using RoeiVereniging.ViewModels;

namespace RoeiVereniging.Views;

public partial class AddUserView : ContentPage
{
	public AddUserView(AddUserViewModel viewmodel)
	{
		InitializeComponent();
        BindingContext = viewmodel;
    }
}