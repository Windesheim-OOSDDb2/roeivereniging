using RoeiVereniging.ViewModels;
using RoeiVereniging.Views;

namespace RoeiVereniging
{
    public partial class App : Application
    {
        public App(AddBoatViewModel viewModel)
        {
            InitializeComponent();
            MainPage = new AppShell();
        }
    }
}
