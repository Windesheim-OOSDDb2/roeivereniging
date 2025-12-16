using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RoeiVereniging.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoeiVereniging.Views;
using RoeiVereniging.Core.Repositories;
using System.Collections.ObjectModel;
using RoeiVereniging.Core.Data.Repositories;

namespace RoeiVereniging.ViewModels
{
    public partial class ReservationDetailViewModel : ObservableObject
    {
        private readonly DamageRepository _damageRepository = new DamageRepository();

        public ObservableCollection<Damage> Damages { get; } = new();

        public ReservationDetailViewModel(ReservationViewDTO reservation)
        {
            Reservation = reservation;
            LoadDamages();
        }

        public void LoadDamages()
        {
            Damages.Clear();
            var damages = _damageRepository.GetByBoatId(BoatId);
            foreach (var damage in damages)
            {
                Damages.Add(damage);
            }
        }

        [ObservableProperty]
        private ReservationViewDTO reservation;

        public int BoatId => Reservation?.BoatId ?? 0;

        public string BoatName => Reservation?.BoatName ?? "";
        public string BoatLevelText => Reservation?.BoatLevelText ?? "";
        public string SteeringModeText => Reservation != null ? Reservation.SteeringMode.ToString() : "";
        public int SeatsAmount => Reservation?.SeatsAmount ?? 0;
        public DateTime StartTime => Reservation?.StartTime ?? DateTime.MinValue;
        public DateTime EndTime => Reservation?.EndTime ?? DateTime.MinValue;


        [RelayCommand]
        private async Task NavigateToReportDamage()
        {
            await Shell.Current.GoToAsync($"{nameof(ReportDamageView)}?boatId={BoatId}");
        }

    }
}
