using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RoeiVereniging.Core.Interfaces.Services;
using RoeiVereniging.Core.Models;
using RoeiVereniging.Views;
using RoeiVereniging.Views.components;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Maui.Views;
using System.Globalization;
using QRCoder;
using static QRCoder.PayloadGenerator;

namespace RoeiVereniging.ViewModels
{
    public partial class ReserveBoatViewModel : BaseViewModel
    {
        public ObservableCollection<Reservation> Reservations { get; set; }
        public ObservableCollection<Boat> Boats { get; set; }
        public ObservableCollection<int> PassengerCounts { get; } = new ObservableCollection<int> { 1, 2, 3, 4, 6, 8 };

        private readonly IReservationService _reservationService;
        private readonly IBoatService _boatService;
        private readonly GlobalViewModel _global;
        private readonly IQrCodeService _qrCodeService;
        public ObservableCollection<BoatType> BoatTypes => Enum.GetValues(typeof(BoatType)).Cast<BoatType>().ToObservableCollection();

        // Put all levels except "Alles" in an collection
        public ObservableCollection<BoatLevel> BoatLevels => Enum.GetValues(typeof(BoatLevel)).Cast<BoatLevel>().Where(level => level != BoatLevel.Alles).ToObservableCollection();


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
        private BoatLevel? difficulty = null;

        [ObservableProperty]
        private BoatType? type = null;

        [ObservableProperty]
        private bool pickerlabel = true;

        [ObservableProperty]
        private string? errorMessage = null;

        private TimeSpan OldTime = DateTime.Now.TimeOfDay;
        private DateTime OldDate = DateTime.Now;


        public ReserveBoatViewModel(IReservationService reservationService, IBoatService boatService, GlobalViewModel global, IQrCodeService qrCodeService)
        {
            _reservationService = reservationService;
            _boatService = boatService;
            _qrCodeService = qrCodeService;
            Reservations = new(_reservationService.GetAll());
            Boats = new ObservableCollection<Boat>(_boatService.GetAll() ?? new List<Boat>());
            _global = global;
        }

        [RelayCommand]
        public async Task ReserveBoat()
        {
            if (!ValidateInputs()) return;

            DateTime ReservationDateTime = date.Date + time;
            Boat? selectedBoat = GetBoat();

            if (selectedBoat == null)
            {
                return;
            }
            else if (_global.currentUser.Role != Role.Admin && _global.currentUser.Role != Role.Materiallcommissaris && _reservationService.GetActiveReservationsCountByUserId(_global.currentUser.Id) >= 2)
            {
                await UpdateErrorUi("Je hebt al 2 actieve reserveringen. Verwijder een bestaande reservering om een nieuwe te maken.");
                return;
            }

            _reservationService.Set(new Reservation(1, _global.currentUser.Id, ReservationDateTime, ReservationDateTime.AddHours(2), DateTime.Now, selectedBoat.Id));

            // Show confirmation popup with reservation details and QR code
            ShowPopup(selectedBoat, ReservationDateTime);

            ResetInputs();
        }

        public void ShowPopup(Boat selectedBoat, DateTime reservationDateTime)
        {
            string titleText = "Reservering bevestigd";
            string dateText = date.ToString("d MMMM yyyy", new CultureInfo("nl-NL"));
            string timeText = time.ToString(@"hh\:mm");
            string popupText = $"De reservering voor {dateText} om {timeText} is succesvol gereserveerd!\nTot dan!";
            string footerText = "Ps. zet de reservering in je eigen agenda!.";
            ImageSource qrCode = GenerateQrCode(selectedBoat, reservationDateTime);

            var popup = new RoeiVereniging.Views.components.ConfirmationPopup(titleText, popupText, footerText, qrCode);
            Shell.Current.CurrentPage.ShowPopup(popup);
        }

        private ImageSource GenerateQrCode(Boat selectedBoat, DateTime reservationDateTime)
        {
            string payload = new CalendarEvent($"Reservering RoeiMeister", $"Roeien met {selectedBoat.name}", "", reservationDateTime, reservationDateTime.AddHours(2), false).ToString();

            return ImageSource.FromStream(() => _qrCodeService.GenerateQrCode(payload));
        }

        public void ResetInputs()
        {
            Amount = 0;
            Date = DateTime.Now.AddDays(7);
            Time = DateTime.Now.TimeOfDay;
            Difficulty = null;
            Type = null;
            ErrorMessage = "";
        }

        public bool ValidateInputs()
        {
            // Check if the reservation date and time is now or later
            DateTime reservationDateTime = date.Date + time;
            if (reservationDateTime < DateTime.Now)
            {
                UpdateErrorUi("Reservering datum en tijd moeten in de toekomst liggen nniet in het verleden.");
                return false;
            }

            if (Amount == 0)
            {
                UpdateErrorUi("Aantal passagiers moet is een verplicht veld.");
                return false;
            }

            if (Difficulty == null)
            {
                UpdateErrorUi("Selecteer een niveau");
                Difficulty = null;
                return false;
            }

            if (Type == null)
            {
                UpdateErrorUi("selecteer een type boot");
                return false;
            }

            return true;
        }

        public Boat? GetBoat()
        {
            if (Type is not BoatType boatType || Difficulty is not BoatLevel boatLevel)
            {
                UpdateErrorUi("Het gegeven type en of niveau is niet geldig");
                return null;
            }

            List<Boat> boats = _boatService.Get(Amount, true, boatLevel, boatType);
            if (boats == null || !boats.Any())
            {
                UpdateErrorUi("Geen passende boot gevonden met de gegeven data");
                return null;
            }

            var availableBoat = CheckAvailability(boats, date.Date + time);
            if (availableBoat == null)
            {
                UpdateErrorUi("Alle boten zijn bezet voor deze datum en tijd probeer een ander moment");
            }

            return availableBoat;
        }

        public Boat? CheckAvailability(List<Boat> boats, DateTime reservationDateTime)
        {
            // Get all reservations that overlap with the given reservationDateTime
            var reservedBoatIds = _reservationService.GetAll()
                .Where(reservation => reservation.StartTime <= reservationDateTime && reservation.EndTime >= reservationDateTime)
                .Select(reservation => reservation.BoatId)
                .ToHashSet();

            // Find the first boat that is not reserved
            return boats.FirstOrDefault(boat => !reservedBoatIds.Contains(boat.Id));
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

        public async Task UpdateErrorUi(string message)
        {
            ErrorMessage = "";
            await Task.Delay(200);
            ErrorMessage = message;
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

        [RelayCommand]
        public async Task GoToBoatList()
        {
            await Shell.Current.GoToAsync(nameof(BoatListView));
        }

        [RelayCommand]
        public async Task Logout()
        {
            await _global.Logout();
        }
    }
}
