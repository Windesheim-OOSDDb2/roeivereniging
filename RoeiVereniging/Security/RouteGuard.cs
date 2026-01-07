using RoeiVereniging.Core.Models;
using RoeiVereniging.Views;
using RoeiVereniging.Views.Admin;

public static class RouteGuard
{
    public static readonly Dictionary<string, Role> ProtectedRoutes = new()
    {
        // ADMIN routes
        { nameof(AddBoatView), Role.Admin },
        { nameof(AdminDashboardView), Role.Admin },
        { nameof(BoatListView), Role.Admin },
        { nameof(UserView), Role.Admin },
        { nameof(RepairView), Role.Admin },
        { nameof(UserDetailView), Role.Admin },

        // USER routes
        { nameof(ReservationView), Role.User },
        { nameof(ReserveBoatView), Role.User }
    };
}