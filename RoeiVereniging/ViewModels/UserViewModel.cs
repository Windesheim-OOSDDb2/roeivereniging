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
        private readonly IUserRepository _userRepository;
        public ObservableCollection<User> Users { get; } = new();

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
                new () { Header = "Voornaam", BindingPath = "FirstName", HeaderType = TableHeaderType.Select },
                new () { Header = "Achternaam", BindingPath = "LastName", HeaderType = TableHeaderType.Select },
                new () { Header = "Registratie datum", BindingPath = "RegistrationDate", HeaderType = TableHeaderType.SortDate, StringFormat = "{0:dd-MM-yyyy}"  },
                new () { Header = "Laatst actief", BindingPath = "LastActiveDate", HeaderType = TableHeaderType.SortDate, StringFormat = "{0:dd-MM-yyyy}"  }
            };
        }

        public void LoadUsers()
        {
            Users.Clear();
            foreach (User user in _userRepository.GetAll())
            {
                Debug.WriteLine($"{user.FirstName}");
                Users.Add(user);
            }
        }

        // sure this is needed?
        public void Refresh()
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
