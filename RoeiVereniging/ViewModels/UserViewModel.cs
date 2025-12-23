using RoeiVereniging.Core.Data.Repositories;
using RoeiVereniging.Core.Interfaces.Services;
using RoeiVereniging.Core.Models;
using RoeiVereniging.Core.Repositories;
using System.Collections.ObjectModel;
using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace RoeiVereniging.ViewModels
{
    public partial class UserViewModel : BaseViewModel
    {
        private readonly UserRepository _userRepo;
        // Get a list of all users from the UserRepository
        private List<UserDTO> _allUsers = new();

        // Make the list DTO's (for UI)
        public ObservableCollection<UserDTO> Users { get; } = new();

        // and expose it as a public property ObservableCollection<User> (so the UI can bind to it)
        public List<string> Names { get; private set; } = new();

        public List<string> LastNames { get; private set; } = new();

        //[ObservableProperty] (CommunityToolkit.Mvvm) to automatically implement INotifyPropertyChanged

        [ObservableProperty]
        private string? selectedName;

        [ObservableProperty]
        private string? selectedLastName;

        [ObservableProperty]
        private int? selectedRegistrationDate;

        [ObservableProperty]
        private int? selectedLastActiveDate;

        [ObservableProperty]
        private string? searchText;

        public UserViewModel(IUserService userService)
        {
            _userRepo = new UserRepository();
            LoadUsers();
        }
 
        private void LoadUsers()
        {
            var users = _userRepo.GetAll();

            foreach (var user in users)
            {
                var userDTO = new UserDTO(
                    user.Id,
                    user.Name
                    //user.LastName,
                    //user.RegistrationDate,
                    //user.LastActiveDate
                    );

                _allUsers.Add(userDTO);
                Users.Add(userDTO);
            }
        }

        partial void OnSearchTextChanged(string? value) => Filter();

        private void Filter()
        {

        }
    }
}
