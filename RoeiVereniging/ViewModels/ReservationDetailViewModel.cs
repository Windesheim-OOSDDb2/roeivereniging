using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using RoeiVereniging.Core.Models;

namespace RoeiVereniging.ViewModels
{
    public partial class ReservationDetailViewModel : ObservableObject
    {
        [ObservableProperty]
        private ReservationViewDTO reservation;

        public string BoatName => Reservation?.BoatName ?? "";
        public string BoatLevelText => Reservation?.BoatLevelText ?? "";
        //public string SteeringModeText => Reservation?.SteeringModeText ?? "";  Not implemented in ReservationViewDTO
        //public int SeatsAmount => Reservation?.SeatsAmount ?? 0; Not implemented in ReservationViewDTO
        public DateTime StartTime => Reservation?.StartTime ?? DateTime.MinValue;
        public DateTime EndTime => Reservation?.EndTime ?? DateTime.MinValue;


        public ReservationDetailViewModel(ReservationViewDTO reservation)
        {
            Reservation = reservation;
        }
    }
}
