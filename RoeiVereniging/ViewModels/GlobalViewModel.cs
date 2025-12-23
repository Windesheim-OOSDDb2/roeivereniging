using RoeiVereniging.Core.Models;

namespace RoeiVereniging.ViewModels
{
    public partial class GlobalViewModel : BaseViewModel
    {
        public required User currentUser { get; set; }
    }
}
