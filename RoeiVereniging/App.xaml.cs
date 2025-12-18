using RoeiVereniging.ViewModels;
using RoeiVereniging.Views;

namespace RoeiVereniging
{
    public partial class App : Application
    {
        public App(LoginView viewModel)
        {
            InitializeComponent();
            MainPage = new NavigationPage(viewModel);
        }
    }
}
