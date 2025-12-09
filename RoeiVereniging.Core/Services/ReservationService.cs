using RoeiVereniging.Core.Interfaces.Repositories;
using RoeiVereniging.Core.Interfaces.Services;
using RoeiVereniging.Core.Models;

namespace RoeiVereniging.Core.Services
{
    public class ReservationService : IReservationService
    {
        private readonly IReservationRepository _reservationRepository;
        public ReservationService(IReservationRepository reservationRepository)
        {
            _reservationRepository = reservationRepository;
        }
        public Reservation? Get(int id)
        {
            return _reservationRepository.Get(id);
        }
        public List<Reservation> GetAll()
        {
            return _reservationRepository.GetAll();
        }
        public Reservation? Set(Reservation reservation)
        {
            return _reservationRepository.Set(reservation);
        }

        public void MarkMessaged(int reservationId)
        {
            _reservationRepository.MarkMessaged(reservationId);
        }

        public List<Reservation> GetUnmessaged()
        {
            return _reservationRepository.GetUnmessaged();
        }

        public List<Reservation> GetByUser(int userId)
        {
            return _reservationRepository.GetByUserId(userId);
        }
    }
}
