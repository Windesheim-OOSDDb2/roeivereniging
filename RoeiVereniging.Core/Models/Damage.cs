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
        public int BoatId { get; set; }
        public string Description { get; set; } = string.Empty;
        public DateTime ReportedAt { get; set; }
        public DamageSeverity Severity { get; set; }
        public Damage() { }
        public Damage(int damageId, int boatId, string description, DateTime reportedAt, DamageSeverity severity)
        {
            DamageId = damageId;
            BoatId = boatId;
            Description = description;
            ReportedAt = reportedAt;
            Severity = severity;
        }
    }
}
