using RoeiVereniging.Core.Models;

namespace RoeiVereniging.ViewModels
{
    public partial class GlobalViewModel : BaseViewModel
    {
        public User? currentUser { get; set; }

        public async Task Logout()
        {
            currentUser = null;
            await Shell.Current.GoToAsync("//LoginView");
        }
    }
}
