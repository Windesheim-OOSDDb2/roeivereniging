using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoeiVereniging.Core.Models
{
    public class RepairDTO
    {
        public int BoatId { get; set; }
        public string BoatName { get; set; }

        public BoatStatus Status { get; set; }

        public string Schade { get; set; } = "Geen schade";

        public DateTime? Notificationdate { get; set; }

    }
}
