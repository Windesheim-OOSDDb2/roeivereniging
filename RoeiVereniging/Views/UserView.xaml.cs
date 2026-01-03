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
    
} 