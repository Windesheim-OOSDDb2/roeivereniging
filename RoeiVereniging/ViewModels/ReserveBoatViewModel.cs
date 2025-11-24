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

        [ObservableProperty]
        private string naam;

        [ObservableProperty]
        private string achternaam;

        [ObservableProperty]
        private string aantalPersonen;

        [ObservableProperty]
        private string email;

        [ObservableProperty]
        private DateTime datum = DateTime.Now.AddDays(7);

        [ObservableProperty]
        private TimeSpan tijd = DateTime.Now.TimeOfDay;

        [ObservableProperty]
        private Boat boat;

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
            if (string.IsNullOrWhiteSpace(Naam) ||
                string.IsNullOrWhiteSpace(Achternaam) ||
                string.IsNullOrWhiteSpace(AantalPersonen) ||
                string.IsNullOrWhiteSpace(Email) ||
                Boat == null)
            {
                // Handle validation error (e.g., show a message to the user)
                return;
            }
            if (!int.TryParse(AantalPersonen, out int passengerCount))
            {
                // TODO: handle invalid input for number
                return;
            }
            var reservation = new Reservation(
                0, // id
                "filler", // name
                int.Parse(AantalPersonen), // passengerCount
                new DateTime(Datum.Year, Datum.Month, Datum.Day, Tijd.Hours, Tijd.Minutes, 0), // dateTime
                1, // userId (should be replaced with actual user ID)
                Boat.Id // boatId
            );

            _reservationService.Set(reservation);
            Reservations.Add(reservation);
            //Clear input fields after reservation
           Naam = string.Empty;
            Achternaam = string.Empty;
            AantalPersonen = string.Empty;
            Email = string.Empty;
            Datum = DateTime.Now.AddDays(7);
            Tijd = DateTime.Now.TimeOfDay;
            Boat = null;
        }

        [RelayCommand]
        public void NewSelectedBoat(Boat product)
        {
            boat = product;
        }
    }
}
