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
        public ObservableCollection<ReservationViewDTO> MyReservations { get; } = new();

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

        // load resevations for dummy user (because no session control yet)
        private void LoadForDummyUser()
        {
            // check if there's an active session with a user (currently using dummy user)
            User? user = _userRepo.GetById(1);
            if (user == null) return;

            // cache boats in dictionary for faster lookup
            List<Boat>? boats = _boatRepo.GetAll();
            var boatById = boats.ToDictionary(b => b.BoatId, b => b.Name);

            List<Reservation> reservations = _reservationService.GetByUser(user.UserId);

            MyReservations.Clear();

            foreach (var reservation in reservations)
            {
                Debug.WriteLine($"Boat count loaded: {_boatRepo.GetAll().Count}");

                string boatName = boatById.TryGetValue(reservation.BoatId, out var bn) ? bn : $"Boat {reservation.BoatId}"; // 🗿
                string userName = user.Name;

                // I used a DTO to store reservation data AND username / boat name for UI binding purposes
                var reservationsList = new ReservationViewDTO(
                    reservation.Id,
                    reservation.UserId,
                    userName,
                    reservation.BoatId,
                    boatName,
                    reservation.StartTime,
                    reservation.EndTime
                );

                // Add to observable collection so ui updates
                MyReservations.Add(reservationsList);
            }
        }
    }
}
