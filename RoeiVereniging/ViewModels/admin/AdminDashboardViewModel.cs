using RoeiVereniging.Core.Interfaces.Services;
using RoeiVereniging.Core.Models;

namespace RoeiVereniging.ViewModels.admin
{
    public partial class AdminDashboardViewModel : BaseViewModel
    {
        private readonly GlobalViewModel _global;
        private readonly IAuthService _auth;

        public AdminDashboardViewModel(
           GlobalViewModel global,
           IAuthService auth)
        {
            _global = global;
            _auth = auth;
        }
        public bool IsAdmin => _global.currentUser != null && _auth.IsAdmin(_global.currentUser);
    }
}
