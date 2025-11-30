using RoeiVereniging.Core.Models;

namespace RoeiVereniging.Core.Interfaces.Repositories
{
    public interface IReservationRepository
    {
        public List<Reservation> GetByUserId(int id);
        public Reservation? Set(Reservation reservation);
        public List<Reservation> GetAll();
        public Reservation? Get(int id);
    }
}