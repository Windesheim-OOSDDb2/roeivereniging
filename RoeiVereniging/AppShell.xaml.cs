using RoeiVereniging.Views;

namespace RoeiVereniging
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(ReservationView), typeof(ReservationView));
            Routing.RegisterRoute(nameof(ReserveBoatView), typeof(ReserveBoatView));
            Routing.RegisterRoute(nameof(AddBoatView), typeof(AddBoatView));
            Routing.RegisterRoute(nameof(WeatherView), typeof(WeatherView));
            Routing.RegisterRoute(nameof(UserView), typeof(UserView));
        }
    }
}
