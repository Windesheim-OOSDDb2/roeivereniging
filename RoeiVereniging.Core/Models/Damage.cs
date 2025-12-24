using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoeiVereniging.Core.Models
{
    public class Damage
    {
        public int DamageId { get; set; }
        public int ReservationId { get; set; }
        public int BoatId { get; set; }
        public int UserId { get; set; }
        public string Description { get; set; } = string.Empty;
        public DateTime ReportedAt { get; set; }
        public EnumDamageSeverity Severity { get; set; }
        public Damage() { }
        public Damage(int damageId, int reservationId, int boatId, int userId, string description, DateTime reportedAt, EnumDamageSeverity severity)
        {
            DamageId = damageId;
            ReservationId = reservationId;
            BoatId = boatId;
            UserId = userId;
            Description = description;
            ReportedAt = reportedAt;
            Severity = severity;
        }
    }
}
