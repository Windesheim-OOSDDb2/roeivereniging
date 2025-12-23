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

            if (_global.currentUser == null || !_auth.IsAdmin(_global.currentUser))
            {
                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    await Shell.Current.GoToAsync("//LoginView");
                });
                return;
            }
        } 
    }
}
