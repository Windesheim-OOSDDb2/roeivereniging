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
        private string firstNameText;

        [ObservableProperty]
        private string lastNameText;

        [ObservableProperty]
        private string emailAdressText;

        [ObservableProperty]
        private DateOnly dateOfBirthText;

        [ObservableProperty]
        private string boatLevelText;

        [ObservableProperty]
        private string userIdText;

        [ObservableProperty]
        private string reservationCountText;

        [ObservableProperty]
        private string damageCountText;

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

            firstNameText = user.FirstName;
            lastNameText = user.LastName;
            emailAdressText = user.EmailAddress;
            dateOfBirthText = user.DateOfBirth;
            boatLevelText = user.Level.ToString();
            userIdText = user.UserId.ToString();
            reservationCountText = _reservationRepository.GetByUserId(userId).Count().ToString();
            damageCountText = _damageRepository.GetByUserId(userId).Count().ToString();

            OnPropertyChanged(nameof(firstNameText));
            OnPropertyChanged(nameof(lastNameText));
            OnPropertyChanged(nameof(emailAdressText));
            OnPropertyChanged(nameof(dateOfBirthText));
            OnPropertyChanged(nameof(boatLevelText));
            OnPropertyChanged(nameof(userIdText));
            OnPropertyChanged(nameof(reservationCountText));
            OnPropertyChanged(nameof(damageCountText));
        }

    }
}
