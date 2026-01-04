using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RoeiVereniging.Core.Data.Repositories;
using RoeiVereniging.Core.Models;
using System.Collections.ObjectModel;
using System.Diagnostics;
using RoeiVereniging.Core.Interfaces.Repositories;
using RoeiVereniging.Core.Repositories;

namespace RoeiVereniging.ViewModels
{
    public partial class UserDetailViewModel : ObservableObject
    {
        private readonly IUserRepository _userRepository = new UserRepository();
        private readonly DamageRepository _damageRepository = new DamageRepository();
        private readonly IReservationRepository _reservationRepository = new ReservationRepository();

        public IRelayCommand DeleteUserCommand { get; }
        public IRelayCommand EditUserCommand { get; }

        [ObservableProperty]
        private string firstName;

        [ObservableProperty]
        private string lastName;

        [ObservableProperty]
        private string emailAddress;

        [ObservableProperty]
        private DateOnly dateOfBirth;

        [ObservableProperty]
        private string boatLevel;

        [ObservableProperty]
        private string reservationCount;

        [ObservableProperty]
        private string damageCount;

        public UserDetailViewModel(int userId)
        {
            LoadUserDetails(userId);
            DeleteUserCommand = new RelayCommand(OnDeleteUser);
            EditUserCommand = new RelayCommand(OnEditUser);
        }
        
        private void OnDeleteUser()
        {
            // Logic to delete user
        }

        private void OnEditUser()
        {
            // Logic to edit user
        }

        private void LoadUserDetails(int userId)
        {
            User user = _userRepository.Get(userId);
            if (user == null)
            {
                Debug.WriteLine($"User with ID {userId} not found.");
                return;
            }

            firstName = user.FirstName;
            lastName = user.LastName;
            emailAddress = user.EmailAddress;
            dateOfBirth = user.DateOfBirth;
            boatLevel = user.Level.ToString();
            reservationCount = _reservationRepository.GetByUserId(userId).Count().ToString();
            //damageCount = _damageRepository.GetByUserId=(userId).Count().ToString();

            OnPropertyChanged(nameof(FirstName));
            OnPropertyChanged(nameof(LastName));
            OnPropertyChanged(nameof(EmailAddress));
            OnPropertyChanged(nameof(DateOfBirth));
            OnPropertyChanged(nameof(BoatLevel));
            OnPropertyChanged(nameof(ReservationCount));
            OnPropertyChanged(nameof(DamageCount));
        }

    }
}
