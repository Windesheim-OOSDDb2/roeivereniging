using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RoeiVereniging.Core.Interfaces.Services;
using RoeiVereniging.Core.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoeiVereniging.ViewModels
{
    public partial class ReserveBoatViewModel : BaseViewModel
    {
        public ObservableCollection<Reservation> Reservations { get; set; }
        public ObservableCollection<Boat> Boats { get; set; }
        private readonly IReservationService _reservationService;
        private readonly IBoatService _boatService;
        public ObservableCollection<BoatType> boatTypes => Enum.GetValues(typeof(BoatType)).Cast<BoatType>().ToObservableCollection();

        [ObservableProperty]
        private DateTime datum = DateTime.Now.AddDays(7);

        [ObservableProperty]
        private TimeSpan tijd = DateTime.Now.TimeOfDay;

        [ObservableProperty]
        private int? amount = null;

        [ObservableProperty]
        private string? difficulty = null;

        [ObservableProperty]
        private BoatType type;

        public ReserveBoatViewModel(IReservationService reservationService, IBoatService boatService)
        {
            _reservationService = reservationService;
            _boatService = boatService;
            Reservations = new(_reservationService.GetAll());
            Boats = new(_boatService.GetAll());
        }

        [RelayCommand]
        public void ReserveerBoot()
        {
            
        }

        [RelayCommand]
        public void NewSelectedBoat(BoatType type)
        {
            Type = type;
        }
    }
}
