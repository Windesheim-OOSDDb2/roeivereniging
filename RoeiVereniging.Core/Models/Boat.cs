using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoeiVereniging.Core.Models
{
    public partial class Boat : Model
    {
        public int MaxPassengers { get; set; }
        public bool Steer {  get; set; }
        public int MinLevel { get; set; }
        public BoatStatus BoatStatus { get; set; } = BoatStatus.Working;

        public Boat(int id, string name, int maxPassengers, bool steer, int minLevel, BoatStatus boatStatus) : base(id, name)
        {
            MaxPassengers = maxPassengers;
            Steer = steer;
            MinLevel = minLevel;
            BoatStatus = boatStatus;
        }
    }
}
