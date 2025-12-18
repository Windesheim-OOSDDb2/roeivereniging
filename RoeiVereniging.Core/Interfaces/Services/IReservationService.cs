using RoeiVereniging.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoeiVereniging.Core.Interfaces.Services
{
    public interface IReservationService
    {
        public Reservation? Get(int id);
        public List<Reservation> GetAll();
        public Reservation? Set(Reservation reservation);
        public void MarkMessaged(int reservationId);
        public List<Reservation> GetUnmessaged();
        public List<Reservation> GetByUser(int userId);
        public List<Reservation> GetByDate(DateTime date);
    }
}
