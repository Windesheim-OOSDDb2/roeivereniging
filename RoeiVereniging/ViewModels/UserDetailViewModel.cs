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
        private readonly IUserRepository _userRepository;
        private readonly IDamageRepository _damageRepository;
        private readonly IReservationRepository _reservationRepository;

        [ObservableProperty]
        private User userData;

        [ObservableProperty]
        private string reservationCountText;

        [ObservableProperty]
        private string damageCountText;

        public UserDetailViewModel(int userId, IUserRepository userRepo, IDamageRepository damageRepo, IReservationRepository reservationRepository)
        {
            _userRepository = userRepo;
            _damageRepository = damageRepo;
            _reservationRepository = reservationRepository;
            LoadUserDetails(userId);
        }

        [RelayCommand]
        private void OnDeleteUser()
        {
            // Logic to delete user
        }

        [RelayCommand]
        public async Task OnEditUser()
        {
            await Shell.Current.GoToAsync($"{nameof(EditUserView)}?id={userData.UserId}");
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
