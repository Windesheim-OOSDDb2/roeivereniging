using RoeiVereniging.Core.Interfaces.Services;
using RoeiVereniging.ViewModels;
using RoeiVereniging.Views;
using RoeiVereniging.Views.Admin;

namespace RoeiVereniging
{
    public partial class AppShell : Shell
    {
        private readonly GlobalViewModel _global;
        private readonly IAuthService _auth;

        public AppShell(GlobalViewModel global, IAuthService auth)
        {   
            InitializeComponent();

            _global = global;
            _auth = auth;

            Navigating += OnShellNavigating;

            Routing.RegisterRoute(nameof(ReservationView), typeof(ReservationView));
            Routing.RegisterRoute(nameof(ReserveBoatView), typeof(ReserveBoatView));
            Routing.RegisterRoute(nameof(AddBoatView), typeof(AddBoatView));
            Routing.RegisterRoute(nameof(WeatherView), typeof(WeatherView));
            Routing.RegisterRoute(nameof(AdminDashboardView), typeof(AdminDashboardView));
            Routing.RegisterRoute(nameof(LoginView), typeof(LoginView));
            Routing.RegisterRoute(nameof(ReservationDetailView), typeof(ReservationDetailView));
            Routing.RegisterRoute(nameof(ReportDamageView), typeof(ReportDamageView));
            Routing.RegisterRoute(nameof(DamageHistoryView), typeof(DamageHistoryView));
            Routing.RegisterRoute(nameof(BoatListView), typeof(BoatListView));
            Routing.RegisterRoute(nameof(AddUserView), typeof(AddUserView));
        }

        private void OnShellNavigating(object sender, ShellNavigatingEventArgs e)
        {
            var targetRoute = e.Target.Location.OriginalString.Split('/').Last();

            // if route is not yet taken into routeguard, there is no protection and any role is required
            if (!RouteGuard.ProtectedRoutes.TryGetValue(targetRoute, out var requiredRole))
            {
                return;
            }

            // block navigation to route when required role doesn't match currentuser
            if (!_auth.CanAccess(_global.currentUser, requiredRole))
            {
                e.Cancel();

                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    await GoToAsync($"///{nameof(LoginView)}");
                });
            }
        }
    }
}
