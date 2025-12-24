namespace RoeiVereniging.Core.Models
{
    // FOR UI BINDING
    public class ReservationViewDTO
    {
        public int ReservationId { get; set; }
        public string BoatName { get; set; } = string.Empty;
        public BoatLevel BoatLevel { get; set; }

        // TO DISPLAY AS TEXT IN UI
        public string BoatLevelText => BoatLevel.ToString();

        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int BoatId { get; set; }
        public int UserId { get; set; }

        public int SeatsAmount { get; set; }
        public SteeringMode SteeringMode { get; set; }

        public ReservationViewDTO() { }

        public ReservationViewDTO(int reservationId, int userId, BoatLevel boatLevel, int boatId, string boatName, DateTime start, DateTime end, int seatsAmount, SteeringMode steeringMode)
        {
            ReservationId = reservationId;
            UserId = userId;
            BoatLevel = boatLevel;
            BoatId = boatId;
            BoatName = boatName;
            StartTime = start;
            EndTime = end;
            SeatsAmount = seatsAmount;
            SteeringMode = steeringMode;
        }
    }
}
