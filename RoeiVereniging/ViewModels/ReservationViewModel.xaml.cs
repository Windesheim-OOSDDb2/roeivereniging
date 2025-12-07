using RoeiVereniging.Core.Data.Repositories;
using RoeiVereniging.Core.Interfaces.Services;
using RoeiVereniging.Core.Models;
using RoeiVereniging.Core.Repositories;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace RoeiVereniging.ViewModels
{
    public partial class ReservationViewModel : BaseViewModel
    {
        // FULL list (unfiltered)
        private List<ReservationViewDTO> _allReservations = new();

        // FILTERED list (bound to UI)
        public ObservableCollection<ReservationViewDTO> MyReservations { get; } = new();

        // DROPDOWN SOURCES
        public List<string> BoatNames { get; private set; } = new();
        public List<string> UserNames { get; private set; } = new();
        public List<DateTime> AvailableDates { get; private set; } = new();
        public List<TimeSpan> AvailableTimes { get; private set; } = new();

        // SELECTED FILTERS
        private string? _selectedBoatName;
        public string? SelectedBoatName
        {
            get => _selectedBoatName;
            set { _selectedBoatName = value; Filter(); OnPropertyChanged(); }
        }

        private string? _selectedUserName;
        public string? SelectedUserName
        {
            get => _selectedUserName;
            set { _selectedUserName = value; Filter(); OnPropertyChanged(); }
        }

        private DateTime? _selectedDate;
        public DateTime? SelectedDate
        {
            get => _selectedDate;
            set { _selectedDate = value; Filter(); OnPropertyChanged(); }
        }

        private TimeSpan? _selectedTime;
        public TimeSpan? SelectedTime
        {
            get => _selectedTime;
            set { _selectedTime = value; Filter(); OnPropertyChanged(); }
        }

        private readonly IReservationService _reservationService;
        private readonly UserRepository _userRepo;
        private readonly BoatRepository _boatRepo;

        public ReservationViewModel(IReservationService reservationService)
        {
            _reservationService = reservationService;
            _userRepo = new UserRepository();
            _boatRepo = new BoatRepository();

            LoadForDummyUser();
        }

        private void LoadForDummyUser()
        {
            User? user = _userRepo.GetById(1);
            if (user == null) return;

            List<Boat>? boats = _boatRepo.GetAll();
            var boatById = boats.ToDictionary(b => b.BoatId, b => b.Name);

            List<Reservation> reservations = _reservationService.GetByUser(user.UserId);

            _allReservations.Clear();
            MyReservations.Clear();

            foreach (var reservation in reservations)
            {
                string boatName = boatById.TryGetValue(reservation.BoatId, out var bn) ? bn : $"Boat {reservation.BoatId}";
                string userName = user.Name;

                var dto = new ReservationViewDTO(
                    reservation.Id,
                    reservation.UserId,
                    userName,
                    reservation.BoatId,
                    boatName,
                    reservation.StartTime,
                    reservation.EndTime
                );

                _allReservations.Add(dto);
                MyReservations.Add(dto);
            }

            // populate dropdown lists
            BoatNames = _allReservations.Select(r => r.BoatName).Distinct().ToList();
            UserNames = _allReservations.Select(r => r.UserName).Distinct().ToList();
            AvailableDates = _allReservations.Select(r => r.StartTime.Date).Distinct().ToList();
            AvailableTimes = _allReservations.Select(r => r.StartTime.TimeOfDay).Distinct().ToList();

            OnPropertyChanged(nameof(BoatNames));
            OnPropertyChanged(nameof(UserNames));
            OnPropertyChanged(nameof(AvailableDates));
            OnPropertyChanged(nameof(AvailableTimes));
        }

        private void Filter()
        {
            IEnumerable<ReservationViewDTO> filtered = _allReservations;

            if (!string.IsNullOrWhiteSpace(SelectedBoatName))
                filtered = filtered.Where(r => r.BoatName == SelectedBoatName);

            if (!string.IsNullOrWhiteSpace(SelectedUserName))
                filtered = filtered.Where(r => r.UserName == SelectedUserName);

            if (SelectedDate != null)
                filtered = filtered.Where(r => r.StartTime.Date == SelectedDate.Value.Date);

            if (SelectedTime != null)
                filtered = filtered.Where(r => r.StartTime.TimeOfDay == SelectedTime.Value);

            MyReservations.Clear();
            foreach (var res in filtered)
                MyReservations.Add(res);
        }
    }
}