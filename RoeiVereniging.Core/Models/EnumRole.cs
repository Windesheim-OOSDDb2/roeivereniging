using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoeiVereniging.Core.Models
{
    public enum Role : ushort
    {
        User = 0,
        Admin = 1,
        Wedstrijdcommissaris = 2,
        Materiallcommissaris = 3
    }
}
    