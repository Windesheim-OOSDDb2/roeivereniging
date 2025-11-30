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
        public ObservableCollection<Reservation> MyReservations { get; } = new();

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
            var user = _userRepo.GetById(1);
            if (user == null) return;

            var Reservations = _reservationService.GetByUser(user.UserId);
            foreach (var reservation in Reservations)
            {
                Debug.WriteLine($"Reservation: {reservation.Id}, Boat ID: {reservation.BoatId}");
            }

            MyReservations.Clear();
            foreach (var reservation in Reservations) MyReservations.Add(reservation);
        }
    }
}
