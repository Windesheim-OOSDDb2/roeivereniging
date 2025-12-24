using RoeiVereniging.Core.Models;
using RoeiVereniging.Views;
using RoeiVereniging.Views.Admin;

public static class RouteGuard
{
    // 
    public static readonly Dictionary<string, Role> ProtectedRoutes = new()
    {
        { nameof(AddBoatView), Role.Admin },
        { nameof(AdminDashboardView), Role.Admin },

        { nameof(ReservationView), Role.User },
        { nameof(ReserveBoatView), Role.User }
    };
}