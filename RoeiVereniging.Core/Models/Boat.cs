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
        public int BoatId { get; set; }
        public int SeatsAmount { get; set; }
        public bool SteeringWheelPosition {  get; set; }
        public BoatLevel Level { get; set; }
        public BoatType Type { get; set; }
        public BoatStatus BoatStatus { get; set; } = BoatStatus.Working;

        public Boat(int id, string name, int seatsAmount, bool steeringWheelPosition, BoatLevel level, BoatStatus boatStatus, BoatType boatType)
            : base(id, name)
        {
            BoatId = id;
            SeatsAmount = seatsAmount;
            SteeringWheelPosition = steeringWheelPosition;
            Level = level;
            BoatStatus = boatStatus;
            Type = boatType;
        }
        public override string? ToString()
        {
            return $"{Name}";
        }
    }
}
