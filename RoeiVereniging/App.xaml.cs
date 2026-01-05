using RoeiVereniging.ViewModels;
using RoeiVereniging.Views;

namespace RoeiVereniging
{
    public partial class App : Application
    {
        public App(AppShell shell)
        {
            InitializeComponent();
            MainPage = shell;
        }

        public GlobalViewModel GlobalViewModel { get; } = new GlobalViewModel();
    }
}
