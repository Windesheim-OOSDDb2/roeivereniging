using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RoeiVereniging.Core.Models;

namespace RoeiVereniging.ViewModels
{
    [QueryProperty(nameof(BoatId), "BoatId")]
    public partial class ReportDamageViewModel : ObservableObject
    {

        [ObservableProperty]
        private string description;

        [ObservableProperty]
        private DamageSeverity severity;

        [ObservableProperty]
        private DateTime reportedAt = DateTime.Now;

        [ObservableProperty]
        private int boatId;

        [ObservableProperty]
        private string feedbackMessage;

        public Array SeverityLevels => Enum.GetValues(typeof(DamageSeverity));

        [RelayCommand]
        private async Task SubmitReport()
        {
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

            // TODO: Schade opslaan via service/repository

            feedbackMessage = "Schade succesvol gerapporteerd. Bedankt!";
        }
    }
}
