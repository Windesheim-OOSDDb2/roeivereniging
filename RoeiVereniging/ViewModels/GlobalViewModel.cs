using RoeiVereniging.Core.Models;

namespace RoeiVereniging.ViewModels
{
    public partial class GlobalViewModel : BaseViewModel
    {
        public User? currentUser { get; set; }

        public void Logout()
        {
            currentUser = null;
        }
    }
}
