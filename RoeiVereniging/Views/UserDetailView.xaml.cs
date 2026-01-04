using RoeiVereniging.ViewModels;

namespace RoeiVereniging.Views;

[QueryProperty(nameof(UserId), "userId")]
public partial class UserDetailView : ContentPage
{
    private int _userId;
    public int UserId
    {
        get => _userId;
        set
        {
            _userId = value;
            BindingContext = new UserDetailViewModel(_userId);
        }
    }
    public UserDetailView()
	{
		InitializeComponent();
    }

    private async void OnBotenClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(BoatListView), true);
    }

    private async void OnGebruikersClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(UserView), true);
    }

    private async void OnReparatiesClicked(object sender, EventArgs e)
    {
        // TODO: Navigatie naar reparatiespagina
    }

    private async void OnOverzichtClicked(object sender, EventArgs e)
    {
        //await Shell.Current.GoToAsync(nameof(AdminDashboardView), true);
    }
}