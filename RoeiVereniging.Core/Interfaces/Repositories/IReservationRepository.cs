using RoeiVereniging.Core.Models;

namespace RoeiVereniging.Core.Interfaces.Repositories
{
    public interface IReservationRepository
    {
        public List<Reservation> GetByUserId(int id);
        public Reservation? Set(Reservation reservation);
        public void MarkMessaged(int reservationId);
        public List<Reservation> GetUnmessaged();
        public List<Reservation> GetAll();
        public int GetActiveReservationsCountByUserId(int id);
        public Reservation Get(int id);
        public List<Reservation> GetByDate(DateTime date);
    }
}