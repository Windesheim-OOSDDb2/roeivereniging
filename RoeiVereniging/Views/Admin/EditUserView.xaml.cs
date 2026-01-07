using RoeiVereniging.ViewModels;
using System.Diagnostics;


namespace RoeiVereniging.Views;

public partial class EditUserView : ContentPage
{
    public EditUserView(EditUserViewModel viewmodel)
	{
		InitializeComponent();
        BindingContext = viewmodel;
    }
}