using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RoeiVereniging.Core.Interfaces.Services;
using RoeiVereniging.Core.Models;
using RoeiVereniging.Views;
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
        private string naam = string.Empty;

        [ObservableProperty]
        private string achternaam = string.Empty;

        [ObservableProperty]
        private string aantalPersonen = string.Empty;

        [ObservableProperty]
        private string email = string.Empty;

        [ObservableProperty]
        private DateTime datum = DateTime.Now.AddDays(7);

        [ObservableProperty]
        private TimeSpan tijd = DateTime.Now.TimeOfDay;

        [ObservableProperty]
        private Boat boat = null!;

        public ReserveBoatViewModel(IReservationService reservationService, IBoatService boatService)
        {
            _reservationService = reservationService;
            _boatService = boatService;
            Reservations = new(_reservationService.GetAll());
            Boats = new ObservableCollection<Boat>(_boatService.GetAll() ?? new List<Boat>());
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
            var reservation = new Reservation(
                0, // id
                1, // user id
                new DateTime(Datum.Year, Datum.Month, Datum.Day, Tijd.Hours, Tijd.Minutes, 0), // startTime
                new DateTime(Datum.Year, Datum.Month, Datum.Day, Tijd.Hours, Tijd.Minutes, 0), // endTime
                DateTime.Now, // createdAt
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

        [RelayCommand]
        public async Task GoToReservations()
        {
            await Shell.Current.GoToAsync(nameof(ReservationView));
        }
    }
}
