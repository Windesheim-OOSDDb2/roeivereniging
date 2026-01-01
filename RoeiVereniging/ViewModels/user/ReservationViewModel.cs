using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RoeiVereniging.Core.Data.Repositories;
using RoeiVereniging.Core.Interfaces.Services;
using RoeiVereniging.Core.Models;
using RoeiVereniging.Core.Repositories;
using RoeiVereniging.Views;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace RoeiVereniging.ViewModels
{
    public partial class ReservationViewModel : BaseViewModel
    {
        private List<ReservationViewDTO> _allReservations = new();
        private readonly GlobalViewModel _global;
        private readonly IAuthService _auth;

        public IList<TableColumnDefinition> ReservationColumns { get; }

        public ObservableCollection<ReservationViewDTO> MyReservations { get; } = new();

        public List<string> BoatNames { get; private set; } = new();
        public List<BoatLevel> Levels { get; private set; } = new();


        private readonly IReservationService _reservationService;
        private readonly UserRepository _userRepo;
        private readonly BoatRepository _boatRepo;

        public ReservationViewModel(IReservationService reservationService, GlobalViewModel global, IAuthService auth)
        {
            _reservationService = reservationService;
            _userRepo = new UserRepository();
            _boatRepo = new BoatRepository();
            _global = global;
            _auth = auth;


            // Fill the columns, the BindingPath must mattch the name of a public property on the object pushed to the table component
            // the binding path is used to read the value ofor displaying the cell and applying filters to the column 
            ReservationColumns = new List<TableColumnDefinition>
            {
                new() { Header = "Bootnaam", BindingPath = "BoatName", HeaderType = TableHeaderType.Select },
                new() { Header = "Niveau", BindingPath = "BoatLevelText", HeaderType = TableHeaderType.Select },
                new() { Header = "Datum", BindingPath = "StartTime", StringFormat = "{0:dd/MM/yyyy}", HeaderType = TableHeaderType.SortDate },
                new() { Header = "Tijd", BindingPath = "StartTime", StringFormat = "{0:HH:mm}", HeaderType = TableHeaderType.SortTime },
            };

            LoadForCurrentUser();
        }

        private void LoadForCurrentUser()
        {
            User? user = _userRepo.Get(_global.currentUser.Id);
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
                        reservation.EndTime,
                        boat.SeatsAmount,
                        boat.SteeringWheelPosition ? SteeringMode.Required : SteeringMode.Disabled
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
    }
}