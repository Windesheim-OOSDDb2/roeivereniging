using RoeiVereniging.Views;

namespace RoeiVereniging
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(ReservationView), typeof(ReservationView));
        }
    }
}
