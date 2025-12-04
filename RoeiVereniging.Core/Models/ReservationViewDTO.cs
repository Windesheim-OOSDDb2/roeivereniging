namespace RoeiVereniging.Core.Models
{
    // FOR UI BINDING
    public class ReservationViewDTO
    {
        public int ReservationId { get; set; }
        public string BoatName { get; set; } = string.Empty;
        public string BoatLevel { get; set; } = string.Empty;
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int BoatId { get; set; }
        public int UserId { get; set; }

        public ReservationViewDTO() { }

        public ReservationViewDTO(int reservationId, int userId, string boatLevel, int boatId, string boatName, DateTime start, DateTime end)
        {
            ReservationId = reservationId;
            UserId = userId;
            BoatLevel = boatLevel;
            BoatId = boatId;
            BoatName = boatName;
            StartTime = start;
            EndTime = end;
        }
    }
}
