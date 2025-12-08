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

        [ObservableProperty]
        private string? selectedUserName;

        [ObservableProperty]
        private bool? selectedDate = null;

        [ObservableProperty]
        private bool? selectedTime = null;

        [ObservableProperty]
        private string dateSortText = @"Datum \/";

        [ObservableProperty]
        private string timeSortText = @"Tijd \/";

        [ObservableProperty]
        private bool niveau = true;

        [ObservableProperty]
        private bool namen = true;


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
            BoatNames.Insert(0, "Bootnaam"); 
            Levels = _allReservations.Select(r => r.Level).Distinct().ToList();
            Levels.Insert(0, 0);
            AvailableDates = _allReservations.Select(r => r.StartTime.Date).Distinct().ToList();
            AvailableTimes = _allReservations.Select(r => r.StartTime.TimeOfDay).Distinct().ToList();

            
        }

        partial void OnSelectedBoatNameChanged(string? selectedBoatName)
        {
            Filter();
            Namen = false;
        }

        partial void OnSelectedLevelChanged(int? selectedlevel)
        {
            Filter();
            Niveau = false;
        }

        partial void OnSelectedDateChanged(bool? value)
        {
            SelectedTime = null;
            Filter();
        }

        partial void OnSelectedTimeChanged(bool? value)
        {
            SelectedDate = null;
            Filter();
        }

        private void Filter()
        {
            IEnumerable<ReservationViewDTO> filtered = _allReservations;

            if (!string.IsNullOrWhiteSpace(SelectedBoatName) && SelectedBoatName != "Bootnaam")
                filtered = filtered.Where(r => r.BoatName == SelectedBoatName);

            if (SelectedLevel!=null&&SelectedLevel>0 && SelectedLevel != 0)
                filtered = filtered.Where(r => r.Level == SelectedLevel);

            if (SelectedDate is not null)
                if (SelectedDate.Value)
                {
                    filtered = filtered.OrderBy(r => r.StartTime);
                    DateSortText = @"Datum /\";
                }
                else
                {
                    filtered = filtered.OrderByDescending(r => r.StartTime);
                    DateSortText = @"Datum \/";
                }

            if (SelectedTime is not null)
                if (SelectedTime.Value)
                {
                    filtered = filtered.OrderBy(r => r.StartTime.TimeOfDay);
                    TimeSortText = @"Tijd /\";
                }
                else
                {
                    filtered = filtered.OrderByDescending(r => r.StartTime.TimeOfDay);
                    TimeSortText = @"Tijd \/";
                }

            MyReservations.Clear();
            foreach (var res in filtered)
                MyReservations.Add(res);
        }
    }
}