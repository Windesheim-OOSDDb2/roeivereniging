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
        public ReservationDetailViewModel(ReservationViewDTO reservation)
        {
            Reservation = reservation;
        }
    }
}
