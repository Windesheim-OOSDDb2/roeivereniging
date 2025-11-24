using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoeiVereniging.Core.Models
{
    public partial class Reservation : Model
    {
        public int PassengerCount { get; set; }
        public DateTime DateTime { get; set; } = DateTime.Now;
        public int UserId { get; set; }
        public int BoatId { get; set; }

        public Reservation(int id, string name, int passengerCount, DateTime dateTime, int userId, int boatId) : base(id, name)
        {
            PassengerCount = passengerCount;
            DateTime = dateTime;
            UserId = userId;
            BoatId = boatId;
        }
    }
}
