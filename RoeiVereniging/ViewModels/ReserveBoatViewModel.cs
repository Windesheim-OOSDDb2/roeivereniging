using CommunityToolkit.Maui.Core.Extensions;
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
        public ObservableCollection<int> PassengerCounts { get; } = new ObservableCollection<int> { 1, 2, 3, 4, 6, 8 };

        private readonly IReservationService _reservationService;
        private readonly IBoatService _boatService;
        public ObservableCollection<BoatType> boatTypes => Enum.GetValues(typeof(BoatType)).Cast<BoatType>().ToObservableCollection();
        

        [ObservableProperty]
        private DateTime date = DateTime.Now.AddDays(7);

        [ObservableProperty]
        private TimeSpan time = DateTime.Now.TimeOfDay;

        [ObservableProperty]
        private String displayTime = "00 : 00";

        [ObservableProperty]
        private String displayDate = "00 - 00 - 1999";

        [ObservableProperty]
        private int amount = 0;

        [ObservableProperty]
        private string? difficulty = null;

        [ObservableProperty]
        private BoatType? type = null;

        [ObservableProperty]
        private bool pickerlabel = true;

        private TimeSpan OldTime = DateTime.Now.TimeOfDay;
        private DateTime OldDate = DateTime.Now;

        public ReserveBoatViewModel(IReservationService reservationService, IBoatService boatService)
        {
            _reservationService = reservationService;
            _boatService = boatService;
            Reservations = new(_reservationService.GetAll());
            Boats = new ObservableCollection<Boat>(_boatService.GetAll() ?? new List<Boat>());
        }

        [RelayCommand]
        public void ReserveBoat()
        {
            if(!ValidateInputs()) return;
            DateTime ReservationDateTime = date.Date + time;
            _reservationService.Set(new Reservation(1, 1, ReservationDateTime, ReservationDateTime.AddHours(2), DateTime.Now, GetBoat().Id));
            ResetInputs();
        }

        public void ResetInputs()
        {
            Amount = 0;
            Date = DateTime.Now.AddDays(7);
            Time = DateTime.Now.TimeOfDay;
            Difficulty = null;
            Type = null;
        }

        public bool ValidateInputs()
        {
            // Check if the reservation date and time is now or later
            DateTime reservationDateTime = date.Date + time;
            if (reservationDateTime < DateTime.Now)
                return false;

            // Check if difficulty can be parsed to an int
            if (!int.TryParse(Difficulty, out _))
            {
                Difficulty = null;
                return false;
            }

            return true;
        }

        public Boat? GetBoat()
        {
            if (Type is BoatType boatType)
            {
                return _boatService.Get(Amount, true, Difficulty, boatType);
            }
            return null;
        }

        [RelayCommand]
        public void NewSelectedBoat(BoatType type)
        {
            Type = type;
        }

        [RelayCommand]
        public void AttributeChanged()
        {
            if (OldTime != time)
            {
                DisplayTime = $"{time.Hours.ToString("D2")} : {time.Minutes.ToString("D2")}";
                OldTime = time;
            }
            if (OldDate != date)
            {
                DisplayDate = $"{date.Day.ToString("d2")} - {date.Month.ToString("D2")} - {date.Year.ToString("D2")}";
                OldDate = date;
            }
            if (Type != null)
            {
                Pickerlabel = false;
            }
        }

        [RelayCommand]
        public async Task GoToReservations()
        {
            await Shell.Current.GoToAsync(nameof(ReservationView));
        }

        [RelayCommand]
        public async Task GoToWeatherPage()
        {
            await Shell.Current.GoToAsync(nameof(WeatherView));
        }
    }
}
