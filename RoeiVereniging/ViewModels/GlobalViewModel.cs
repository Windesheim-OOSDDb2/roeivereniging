using RoeiVereniging.Core.Models;

namespace RoeiVereniging.ViewModels
{
    public partial class GlobalViewModel : BaseViewModel
    {
        public required User user { get; set; }

        public bool IsAdmin => user?.Role == Role.Admin;
    }
}
