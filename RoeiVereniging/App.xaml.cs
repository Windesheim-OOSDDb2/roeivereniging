using RoeiVereniging.ViewModels;
using RoeiVereniging.Views;

namespace RoeiVereniging
{
    public partial class App : Application
    {
        public App(LoginViewModel viewModel)
        {
            InitializeComponent();

            //MainPage = new AppShell();
            MainPage = new LoginView(viewModel);
        }
    }
}
