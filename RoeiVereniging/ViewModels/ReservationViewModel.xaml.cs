using RoeiVereniging.Core.Data.Repositories;
using RoeiVereniging.Core.Interfaces.Services;
using RoeiVereniging.Core.Models;
using RoeiVereniging.Core.Repositories;
using System.Collections.ObjectModel;
using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;

namespace RoeiVereniging.ViewModels
{
    public partial class ReservationViewModel : BaseViewModel
    {
        
        private List<ReservationViewDTO> _allReservations = new();

        
        public ObservableCollection<ReservationViewDTO> MyReservations { get; } = new();

        
        public List<string> BoatNames { get; private set; } = new();
        public List<int> Levels { get; private set; } = new();

        public List<DateTime> AvailableDates { get; private set; } = new();
        public List<TimeSpan> AvailableTimes { get; private set; } = new();

        [ObservableProperty]
        private string? selectedBoatName;

        [ObservableProperty]
        private int? selectedLevel;

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
            // removed creation of dict since more data is needed from the boat object and the complete object will now be retrieved later

            List<Reservation> reservations = _reservationService.GetByUser(user.UserId);

            _allReservations.Clear();
            MyReservations.Clear();

            foreach (var reservation in reservations)
            {
                // replaced boatname var with boat var wich containt complete boat object so all other value can also be retrieved
                Boat? boat = boats.FirstOrDefault(x => x.BoatId ==  reservation.BoatId);
                string userName = user.Name;

                var dto = new ReservationViewDTO(
                    reservation.Id,
                    reservation.UserId,
                    userName,
                    reservation.BoatId,
                    boat.name,
                    reservation.StartTime,
                    reservation.EndTime,
                    boat.Level
                );

                _allReservations.Add(dto);
                MyReservations.Add(dto);
            }

            // populate dropdown lists
            BoatNames = _allReservations.Select(r => r.BoatName).Distinct().ToList();
            Levels = _allReservations.Select(r => r.Level).Distinct().ToList();
            AvailableDates = _allReservations.Select(r => r.StartTime.Date).Distinct().ToList();
            AvailableTimes = _allReservations.Select(r => r.StartTime.TimeOfDay).Distinct().ToList();

            
        }

        partial void OnSelectedBoatNameChanged(string? selectedBoatName)
        {
            Filter();
        }

        partial void OnSelectedLevelChanged(int? selectedlevel)
        {
            Filter();
        }

        private void Filter()
        {
            IEnumerable<ReservationViewDTO> filtered = _allReservations;

            if (!string.IsNullOrWhiteSpace(SelectedBoatName))
                filtered = filtered.Where(r => r.BoatName == SelectedBoatName);

            if (SelectedLevel!=null&&SelectedLevel>0)
                filtered = filtered.Where(r => r.Level == SelectedLevel);

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