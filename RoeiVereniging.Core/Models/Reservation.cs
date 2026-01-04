
namespace RoeiVereniging.Core.Models
{
    public partial class Reservation : Model
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public DateTime CreatedAt { get; set; }
        public int UserId { get; set; }
        public int BoatId { get; set; }
        public int Messaged { get; set; }

        public Reservation(int reservation_id, int userId, DateTime startTime, DateTime endTime, DateTime createdAt, int boatId)
            : base(reservation_id, "reservering")
        {
            StartTime = startTime;
            EndTime = endTime;
            CreatedAt = createdAt;
            UserId = userId;
            BoatId = boatId;
        }

        public Reservation(int reservation_id, int userId, DateTime startTime, DateTime endTime, DateTime createdAt, int boatId, int messaged)
            : base(reservation_id, "reservering")
        {
            StartTime = startTime;
            EndTime = endTime;
            CreatedAt = createdAt;
            UserId = userId;
            BoatId = boatId;
            Messaged = messaged;
        }
    }
}
