using RoeiVereniging.Core.Data.Repositories;
using RoeiVereniging.Core.Interfaces.Services;
using RoeiVereniging.Core.Models;
using RoeiVereniging.Core.Repositories;
using System.Collections.ObjectModel;
using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace RoeiVereniging.ViewModels
{
    public partial class ReservationViewModel : BaseViewModel
    {
        private List<ReservationViewDTO> _allReservations = new();

        public ObservableCollection<ReservationViewDTO> MyReservations { get; } = new();

        public List<string> BoatNames { get; private set; } = new();
        public List<BoatLevel> Levels { get; private set; } = new();

        [ObservableProperty]
        private string? selectedBoatName;

        [ObservableProperty]
        private BoatLevel? selectedLevel;

        [ObservableProperty]
        private bool? selectedDate = null;

        [ObservableProperty]
        private bool? selectedTime = null;

        [ObservableProperty]
        private string dateSortText = @"Datum \/";

        [ObservableProperty]
        private string timeSortText = @"Tijd \/";

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

            var boats = _boatRepo.GetAll();
            var boatById = boats.ToDictionary(b => b.BoatId, b => b);

            var reservations = _reservationService.GetByUser(user.Id);

            _allReservations.Clear();
            MyReservations.Clear();

            foreach (var reservation in reservations)
            {
                if (boatById.TryGetValue(reservation.BoatId, out var boat))
                {
                    var dto = new ReservationViewDTO(
                        reservation.Id,
                        reservation.UserId,
                        boat.Level,
                        reservation.BoatId,
                        boat.Name,
                        reservation.StartTime,
                        reservation.EndTime
                    );

                    _allReservations.Add(dto);
                    MyReservations.Add(dto);
                }
            }

            BoatNames = _allReservations
                .Select(r => r.BoatName)
                .Distinct()
                .ToList();

            // I insert a None level to make sure user can reset filter
            BoatNames.Insert(0, "Bootnaam");

            Levels = _allReservations
                .Select(r => r.BoatLevel)
                .Distinct()
                .ToList();

            // I insert a None level to make sure user can reset filter
            Levels.Insert(0, BoatLevel.Alles);

            OnPropertyChanged(nameof(BoatNames));
            OnPropertyChanged(nameof(Levels));
        }

        partial void OnSelectedBoatNameChanged(string? value) => Filter();
        partial void OnSelectedLevelChanged(BoatLevel? value) => Filter();

        [RelayCommand]
        private void ToggleDate()
        {
            SelectedTime = null;
            SelectedDate = SelectedDate == true ? false : true;
            Filter();
        }

        [RelayCommand]
        private void ToggleTime()
        {
            SelectedDate = null;
            SelectedTime = SelectedTime == true ? false : true;
            Filter();
        }

        private void Filter()
        {
            IEnumerable<ReservationViewDTO> filtered = _allReservations;

            if (!string.IsNullOrWhiteSpace(SelectedBoatName) && SelectedBoatName != "Bootnaam")
                filtered = filtered.Where(r => r.BoatName == SelectedBoatName);

            if (SelectedLevel != null && SelectedLevel != BoatLevel.Alles)
                filtered = filtered.Where(r => r.BoatLevel == SelectedLevel);

            if (SelectedDate is not null)
            {
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
            }

            if (SelectedTime is not null)
            {
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
            }
            MyReservations.Clear();
            foreach (var res in filtered)
                MyReservations.Add(res);
        }
    }
}