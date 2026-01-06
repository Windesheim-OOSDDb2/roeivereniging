using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RoeiVereniging.Core.Helpers;
using RoeiVereniging.Core.Interfaces.Services;
using RoeiVereniging.Core.Models;
using RoeiVereniging.Views;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace RoeiVereniging.ViewModels
{
    [QueryProperty(nameof(UserId), "id")]
    public partial class EditUserViewModel : BaseViewModel
    {
        private readonly IUserService _userService;

        public ObservableCollection<BoatLevel> BoatLevels { get; } = new(Enum.GetValues<BoatLevel>());
        public ObservableCollection<Role> Roles { get; } = new(Enum.GetValues<Role>());

        [ObservableProperty]
        private int userId;

        [ObservableProperty]
        private string? firstName;

        [ObservableProperty]
        private string? lastName;

        [ObservableProperty]
        private string? email;

        [ObservableProperty]
        private string? password;

        [ObservableProperty]
        private DateTime dateOfBirth;
        [ObservableProperty]
        private BoatLevel selectedBoatLevel;
        [ObservableProperty]
        private Role selectedRole;

        [ObservableProperty]
        private string? dateOfBirthDisplay;

        [ObservableProperty]
        private string? errorMessage;

        public EditUserViewModel(IUserService userService)
        {
            _userService = userService;
        }

        partial void OnUserIdChanged(int value)
        {
            var user = _userService.Get(value);
            if (user != null)
            {
                FirstName = user.FirstName;
                LastName = user.LastName;
                Email = user.EmailAddress;
                Password = ""; // Do not prefill password for security
                DateOfBirth = user.DateOfBirth.ToDateTime(TimeOnly.MinValue);
                SelectedBoatLevel = user.Level;
                SelectedRole = user.Role;
                Debug.WriteLine("User loaded successfully.");
            }
            else
            {
                Debug.WriteLine("User could not be loaded.");
            }
        }

        [RelayCommand]
        public void EditUser()
        {
            if (!ValidateInputs()) return;

            // Retrieve the existing user
            var existingUser = _userService.Get(UserId);
            if (existingUser == null)
            {
                ErrorMessage = "Gebruiker niet gevonden.";
                return;
            }

            // Update user password if it has changed
            string hashedPassword = existingUser.Password;
            if (!string.IsNullOrEmpty(Password) && !PasswordHelper.VerifyPassword(Password, existingUser.Password))
            {
                hashedPassword = PasswordHelper.HashPassword(Password);
            } else
            {
                hashedPassword = hashedPassword;
            }

            var editedUser = _userService.Update(new User(
                UserId,
                FirstName!,
                LastName!,
                Email!,
                hashedPassword,
                SelectedRole,
                SelectedBoatLevel,
                DateOnly.FromDateTime(DateOfBirth),
                existingUser.RegistrationDate,
                DateTime.Now
            ));

            ShowSuccessPopup(editedUser!);
            GoBack();
        }


        [RelayCommand]
        public async void GoBack()
        {
            await Shell.Current.GoToAsync(nameof(UserView));
        }

        public void ShowSuccessPopup(User editedUser)
        {
            string titleText = "Gebruiker succesvol aangepast";
            string popupText = $"De gebruiker {editedUser.Name} met e-mailadres {editedUser.EmailAddress} is succesvol aangepast!";
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
            else if (_userService.Get(Email) != null && _userService.Get(Email)!.UserId != UserId)
            {
                ErrorMessage = "Email is al in gebruik.";
                return false;
            }
            if (Password.Length < 8 && Password.Length > 0)
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
