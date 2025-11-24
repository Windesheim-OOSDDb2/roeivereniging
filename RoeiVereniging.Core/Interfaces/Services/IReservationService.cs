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
        public Reservation? Get(string name);
        public Reservation? Get(int id);
        public List<Boat> GetAll();
        public Reservation? Set(Reservation reservation);
    }
}
