namespace RoeiVereniging.ViewModels
{
    // FOR UI BINDING
    public class ReservationViewDTO
    {
        public int ReservationId { get; set; }
        public string BoatName { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int BoatId { get; set; }
        public int UserId { get; set; }
        public int Level { get; set; }

        public ReservationViewDTO() { }

        public ReservationViewDTO(int reservationId, int userId, string userName, int boatId, string boatName, DateTime start, DateTime end, int level)
        {
            ReservationId = reservationId;
            UserId = userId;
            UserName = userName;
            BoatId = boatId;
            BoatName = boatName;
            StartTime = start;
            EndTime = end;
            Level = level;
        }
    }
}
