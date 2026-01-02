using Microsoft.Maui.Controls;
using RoeiVereniging.ViewModels;
using System.Threading.Tasks;

namespace RoeiVereniging.Views;

public partial class UserView : ContentPage
{
    public UserView()
    {
        InitializeComponent();
        BindingContext = new UserViewModel();
    }
    private async void OnBotenClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(BoatListView), true);
    }

    private async void OnGebruikersClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(UserView), true);
    }

    private void OnReparatiesClicked(object sender, EventArgs e)
    {
        // TODO: Navigatie naar reparatiespagina
    }

    private void OnOverzichtClicked(object sender, EventArgs e)
    {
        // TODO: Navigatie naar overzichtspagina
    }
} 