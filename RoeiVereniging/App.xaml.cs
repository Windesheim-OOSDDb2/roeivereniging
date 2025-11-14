using RoeiVereniging.ViewModels;
using RoeiVereniging.Views;

namespace RoeiVereniging
{
    public partial class App : Application
    {
        public App(StartViewModel viewModel)
        {
            InitializeComponent();

            MainPage = new StartView(viewModel);
        }
    }
}
