using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RoeiVereniging.Core.Models;
using RoeiVereniging.Core.Data.Repositories;
using System.Diagnostics;
using System.Collections.ObjectModel;
using RoeiVereniging.Core.Interfaces.Services;
using RoeiVereniging.Core.Services;
using RoeiVereniging.Core.Repositories;
using CommunityToolkit.Maui.Views;

namespace RoeiVereniging.ViewModels
{
    [QueryProperty(nameof(BoatId), "BoatId")]
    [QueryProperty(nameof(ReservationId), "ReservationId")]
    public partial class ReportDamageViewModel : ObservableObject
    {

        private readonly DamageRepository _damageRepository = new DamageRepository();
        private readonly BoatRepository _boatRepository = new BoatRepository();
        private readonly IUserService _userService = new UserService(new UserRepository());

        [ObservableProperty]
        private string description;

        [ObservableProperty]
        private EnumDamageSeverity severity;

        [ObservableProperty]
        private DateTime reportedAt = DateTime.Now;

        [ObservableProperty]
        private int boatId;

        [ObservableProperty]
        private string boatName;

        [ObservableProperty]
        private int reservationId;

        [ObservableProperty]
        private string feedbackMessage;



        public Array SeverityLevels => Enum.GetValues(typeof(EnumDamageSeverity));

        partial void OnBoatIdChanged(int value)
        {
            var boat = _boatRepository.Get(value);
            BoatName = boat?.ToString() ?? string.Empty;
            OnPropertyChanged(nameof(BoatDisplayText));
        }

        partial void OnBoatNameChanged(string value)
        {
            OnPropertyChanged(nameof(BoatDisplayText));
        }

        partial void OnDescriptionChanged(string value)
        {
            FeedbackMessage = string.Empty;
        }

        partial void OnSeverityChanged(EnumDamageSeverity value)
        {
            FeedbackMessage = string.Empty;
        }

        [RelayCommand]
        private async Task SubmitReport()
        {
            if (string.IsNullOrWhiteSpace(Description))
            {
                FeedbackMessage = "Beschrijving is verplicht.";
                return;
            }

            var currentUser = _userService.GetAll().FirstOrDefault();
            if (currentUser == null)
            {
                FeedbackMessage = "Gebruiker niet gevonden. Log opnieuw in.";
                return;
            }

            var damage = new Damage
            {
                BoatId = BoatId,
                Description = Description,
                Severity = Severity,
                ReportedAt = ReportedAt,
                UserId = currentUser.Id,
                ReservationId = ReservationId
            };

            _damageRepository.Add(damage);

            FeedbackMessage = "Schade succesvol gerapporteerd. Bedankt!";
            Description = string.Empty;

            var popup = new RoeiVereniging.Views.components.ConfirmationPopup("Schade succesvol gemeld!", "De schade is succesvol binnengekomen en we gaan er zo spoedig mogelijk mee bezig.", "Nogmaals bedankt voor het melden.");
            Shell.Current.CurrentPage.ShowPopup(popup);

            await Shell.Current.GoToAsync("..");
        }

        public string BoatDisplayText => $"{BoatName} (ID: {BoatId})";


    }

}
