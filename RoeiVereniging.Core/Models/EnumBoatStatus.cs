using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoeiVereniging.Core.Models
{
    public enum BoatStatus : ushort
    {
        Working = 0,
        Broken = 1,
        Fixing = 2,
        Archived = 3
    }
}
