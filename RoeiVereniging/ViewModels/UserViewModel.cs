using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RoeiVereniging.Core.Data.Repositories;
using RoeiVereniging.Core.Interfaces.Repositories;
using RoeiVereniging.Core.Interfaces.Services;
using RoeiVereniging.Core.Models;
using RoeiVereniging.Core.Repositories;
using RoeiVereniging.Views;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace RoeiVereniging.ViewModels
{
    public partial class UserViewModel : BaseViewModel
    {
        private readonly IUserRepository _userRepository;

        public ICommand RowClickedCommand => new Command<User>(async user =>
        {
            // Await the previous navigation to complete before starting a new one
            try
            {
                await Shell.Current.GoToAsync($"/UserDetailView?userId={user.UserId}");
            }
            catch (InvalidOperationException ex)
            {
                Debug.WriteLine($"navigation error: {ex.Message}");
            }
        });

        public ObservableCollection<User> Users { get; } = new();

        public ICommand RowClickedCommand => new Command<User>(async user =>
        {
            try
            {
                await Shell.Current.GoToAsync($"{nameof(EditUserView)}?id={user.UserId}");
            }
            catch (InvalidOperationException ex)
            {
                System.Diagnostics.Debug.WriteLine($"navigation error: {ex.Message}");
            }
        });

        public IList<TableColumnDefinition> UserTableColumns { get; }


        public UserViewModel()
        {
            _userRepository = new UserRepository();
            LoadUsers();

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
                Users.Add(user);
            }
        }

        [RelayCommand]
        private async Task GoToAddUser()
        {
            await Shell.Current.GoToAsync(nameof(AddUserView));
        }
    }
}
