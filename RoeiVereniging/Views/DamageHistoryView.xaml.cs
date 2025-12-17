using RoeiVereniging.ViewModels;

namespace RoeiVereniging.Views;

public partial class DamageHistoryView : ContentPage
{
	public DamageHistoryView()
	{
		InitializeComponent();
		BindingContext = new DamageHistoryViewModel();
	}
}