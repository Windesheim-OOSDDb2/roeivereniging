using RoeiVereniging.Core.Data.Repositories;
using RoeiVereniging.Core.Interfaces.Services;
using RoeiVereniging.Core.Models;
using RoeiVereniging.Core.Repositories;
using System.Collections.ObjectModel;
using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RoeiVereniging.Core.Interfaces.Repositories;
using RoeiVereniging.Views;
using System.ComponentModel;
using System.Windows.Input;

namespace RoeiVereniging.ViewModels
{
    public partial class UserViewModel : BaseViewModel
    {
        private readonly UserRepository _userRepo;
        // Get a list of all users from the UserRepository
        private List<UserDTO> _allUsers = new();

        //// Make the list DTO's (for UI)
        //public ObservableCollection<UserDTO> Users { get; } = new();

        //// and expose it as a public property ObservableCollection<User> (so the UI can bind to it)
        //public List<string> Names { get; private set; } = new();

        //public List<string> LastNames { get; private set; } = new();

        ////[ObservableProperty] (CommunityToolkit.Mvvm) to automatically implement INotifyPropertyChanged

        //[ObservableProperty]
        //private string? selectedName;

        //[ObservableProperty]
        //private string? selectedLastName;

        //[ObservableProperty]
        //private int? selectedRegistrationDate;

        //[ObservableProperty]
        //private int? selectedLastActiveDate;

        //[ObservableProperty]
        //private string? searchText;

        //public UserViewModel(IUserService userService)
        //{
        //    _userRepo = new UserRepository();
        //    LoadUsers();
        //}

        //private void LoadUsers()
        //{
        //    var users = _userRepo.GetAll();

        //    foreach (var user in users)
        //    {
        //        var userDTO = new UserDTO(
        //            user.Id,
        //            user.Name
        //            //user.LastName,
        //            //user.RegistrationDate,
        //            //user.LastActiveDate
        //            );

        //        _allUsers.Add(userDTO);
        //        Users.Add(userDTO);
        //    }
        //}

        //partial void OnSearchTextChanged(string? value) => Filter();

        //private void Filter()
        //{

        //}

        private readonly IUserRepository _userRepository;
        public List<User> Users { get; set; } = new();

        public IList<TableColumnDefinition> UserTableColumns { get; }

        public ICommand GoToAddUserCommand { get; }

        public UserViewModel()
        {
            _userRepository = new UserRepository();
            LoadUsers();

            // Searchbar filtering implemented here

            //GoToAddUserCommand = new Command(async () =>
            //{
            //    await Shell.Current.GoToAsync(nameof(AddUserPage));
            //});

            UserTableColumns = new List<TableColumnDefinition>
            {
                new () { Header = "ID", BindingPath = "UserId", HeaderType = TableHeaderType.Select },
                new () { Header = "Naam", BindingPath = "Name", HeaderType = TableHeaderType.Select }
                //new () { Header = "Achternaam", BindingPath = "-", HeaderType = TableHeaderType.Select },
                //new () { Header = "Registratiedatum", BindingPath = "-", HeaderType = TableHeaderType.Select },
                //new () { Header = "Laatst actief", BindingPath = "-", HeaderType = TableHeaderType.Select }
            };
        }

        public void LoadUsers()
        {
            Users.Clear();
            foreach (var user in _userRepository.GetAll())
            {
                Users.Add(user);
            }
        }

        public void RefreshUsers()
        {
            LoadUsers();
        }

        //public async void GoToAddUserPage()
        //{
        //    // Implement navigation to AddUserPage
        //    // await Shell.Current.GoToAsync(nameof(AddUserPage));
        //}
    }
}
