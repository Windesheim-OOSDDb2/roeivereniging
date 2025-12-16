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

namespace RoeiVereniging.ViewModels
{
    [QueryProperty(nameof(BoatId), "BoatId")]
    public partial class ReportDamageViewModel : ObservableObject
    {

        private readonly DamageRepository _damageRepository = new DamageRepository();
        private readonly BoatRepository _boatRepository = new BoatRepository();

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
        private string feedbackMessage;



        public Array SeverityLevels => Enum.GetValues(typeof(EnumDamageSeverity));

        partial void OnBoatIdChanged(int value)
        {
            var boat = _boatRepository.Get(value);
            BoatName = boat?.ToString() ?? string.Empty;
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
            Debug.WriteLine("wuggelwug");
            if (string.IsNullOrWhiteSpace(Description))
            {
                FeedbackMessage = "Beschrijving is verplicht.";
                return;
            }

            var damage = new Damage
            {
                BoatId = BoatId,
                Description = Description,
                Severity = Severity,
                ReportedAt = ReportedAt
            };

            _damageRepository.Add(damage);

            FeedbackMessage = "Schade succesvol gerapporteerd. Bedankt!";
            Description = string.Empty;

            
            await Shell.Current.GoToAsync("..");


        }

    }
}
