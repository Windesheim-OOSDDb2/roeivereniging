using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RoeiVereniging.Core.Helpers;
using RoeiVereniging.Core.Interfaces.Services;
using RoeiVereniging.Core.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace RoeiVereniging.ViewModels
{
    public partial class AddUserViewModel : BaseViewModel
    {
        private readonly IUserService _userService;

        public ObservableCollection<BoatLevel> BoatLevels { get; } = new ObservableCollection<BoatLevel>(Enum.GetValues<BoatLevel>());
        public ObservableCollection<Role> Roles { get; } = new ObservableCollection<Role>(Enum.GetValues<Role>());

        [ObservableProperty]
        private string? firstName;

        [ObservableProperty]
        private string? lastName;

        [ObservableProperty]
        private string? email;

        [ObservableProperty]
        private string? password;

        [ObservableProperty]
        private DateTime dateOfBirth = DateTime.Now;

        [ObservableProperty]
        private BoatLevel selectedBoatLevel = BoatLevel.Beginner;

        [ObservableProperty]
        private Role selectedRole;

        [ObservableProperty]
        private string? dateOfBirthDisplay = DateTime.Now.ToString("dd-MM-yyyy");

        [ObservableProperty]
        private string? errorMessage;

        public AddUserViewModel(IUserService userService)
        {
            _userService = userService;
        }

        [RelayCommand]
        public void AddUser()
        {
            if (!ValidateInputs()) return;
            User newUser = _userService.Set(new User(0, FirstName!, LastName!, Email!, PasswordHelper.HashPassword(Password!), SelectedRole, SelectedBoatLevel, DateOnly.FromDateTime(DateOfBirth), DateTime.Now, DateTime.Now));
            ShowSuccessPopup(newUser);
            GoBack();
        }

        [RelayCommand]
        public async void GoBack()
        {
            await Shell.Current.GoToAsync("..");
        }

        public void ShowSuccessPopup(User newUser)
        {
            string titleText = "Gebruiker succesvol toegevoegd";
            string popupText = $"De gebruiker {newUser.Name} met e-mailadres {newUser.EmailAddress} is succesvol toegevoegd!";
            string footerText = "Klik op klaar om terug te gaan naar het vorige scherm";

            var popup = new RoeiVereniging.Views.components.ConfirmationPopup(titleText, popupText, footerText, null);
            Shell.Current.CurrentPage.ShowPopup(popup);
        }

        public bool ValidateInputs()
        {
            if (string.IsNullOrWhiteSpace(FirstName))
            {
                ErrorMessage = "Voornaam is verplicht.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(LastName))
            {
                ErrorMessage = "Achternaam is verplicht.";
                return false;
            }
            if (string.IsNullOrWhiteSpace(Email))
            {
                ErrorMessage = "Email is verplicht.";
                return false;
            }
            else if (!System.Text.RegularExpressions.Regex.IsMatch(Email, @"^[^@\s]+@[^@\s]+\.[a-zA-Z]{2,3}$"))
            {
                ErrorMessage = "Email is niet geldig";
                return false;
            }
            else if(_userService.Get(Email) != null)
            {
                ErrorMessage = "Email is al in gebruik.";
                return false;
            }
            if (string.IsNullOrWhiteSpace(Password))
            {
                ErrorMessage = "Wachtwoord is verplicht.";
                return false;
            }
            else if (Password.Length < 8)
            {
                ErrorMessage = "Wachtwoord moet minstens 8 tekens lang zijn.";
                return false;
            }

            if (DateOfBirth >= DateTime.Now)
            {
                ErrorMessage = "Geboortedatum moet in het verleden liggen.";
                return false;
            }

            return true;
        }

        partial void OnDateOfBirthChanged(DateTime dateOfBirth)
        {
            DateOfBirthDisplay = dateOfBirth.ToString("dd-MM-yyyy");
        }
    }
}
