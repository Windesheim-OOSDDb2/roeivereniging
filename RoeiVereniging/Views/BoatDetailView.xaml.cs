using RoeiVereniging.ViewModels;

namespace RoeiVereniging.Views;

[QueryProperty(nameof(BoatId), "boatId")]
public partial class BoatDetailView : ContentPage
{
    private int _boatId;
    public int BoatId
    {
        get => _boatId;
        set
        {
            _boatId = value;
            BindingContext = new BoatDetailViewModel(_boatId);
        }
    }

    public BoatDetailView()
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
}