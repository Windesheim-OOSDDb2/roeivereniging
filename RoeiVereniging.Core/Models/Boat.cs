using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoeiVereniging.Core.Models
{
    public partial class Boat : Model
    {
        public int Level { get; set; }
        public int SeatsAmount { get; set; }
        public bool SteeringWheelPosition {  get; set; }
        public BoatType BoatType { get; set; } = BoatType.Roeiboot;
        public BoatStatus BoatStatus { get; set; } = BoatStatus.Working;

        public Boat(int id, string name, int seatsAmount, bool steeringWheelPosition, int level, BoatStatus boatStatus, BoatType boat) 
            : base(id, name)
        {
            SeatsAmount = seatsAmount;
            SteeringWheelPosition = steeringWheelPosition;
            Level = level;
            BoatStatus = boatStatus;
        }
        public override string? ToString()
        {
            return $"{Name}";
        }
    }
}
