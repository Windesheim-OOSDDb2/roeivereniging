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
using RoeiVereniging.Views;

namespace RoeiVereniging.ViewModels
{
    public partial class UserDetailViewModel : ObservableObject
    {
        private readonly IUserRepository _userRepository = new UserRepository();
        private readonly DamageRepository _damageRepository = new DamageRepository();
        private readonly IReservationRepository _reservationRepository = new ReservationRepository();

        [ObservableProperty]
        private User userData;

        [ObservableProperty]
        private string reservationCountText;

        [ObservableProperty]
        private string damageCountText;

        public UserDetailViewModel(int userId)
        {
            LoadUserDetails(userId);
        }

        [RelayCommand]
        private void OnDeleteUser()
        {
            // Logic to delete user
        }

        [RelayCommand]
        private void OnEditUser()
        {
            // Logic to edit user
        }

        private void LoadUserDetails(int userId)
        {
            User user = _userRepository.Get(userId);
            if (user == null)
            {
                return;
            }

            userData = user;
            reservationCountText = _reservationRepository.GetByUserId(userId).Count().ToString();
            damageCountText = _damageRepository.GetByUserId(userId).Count().ToString();

            OnPropertyChanged(nameof(userData));
            OnPropertyChanged(nameof(reservationCountText));
            OnPropertyChanged(nameof(damageCountText));
        }
    }
}
