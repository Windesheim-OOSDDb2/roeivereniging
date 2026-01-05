using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoeiVereniging.Core.Models
{
    public enum BoatStatus : ushort
    {
        Werkend = 0,
        Kapot = 1,
        Onderhoud = 2,
        Gearchiveerd = 3
    }
}
