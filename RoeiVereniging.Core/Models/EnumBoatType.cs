using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoeiVereniging.Core.Models
{
    public enum BoatType : ushort
    {
        [Description("1x")]
        onex = 0,
        [Description("2x")]
        twox = 1,
        [Description("4x-")]
        fourxmin = 2,
        [Description("4x+")]
        fourxplus = 3,
        [Description("C1x")]
        Conex = 4,
        [Description("C2x")]
        Ctwox = 5,
        [Description("C2x+")]
        Ctwoxplus = 6,
        [Description("C4x+")]
        Cfourxplus = 7,
        [Description("2-")]
        twomin = 8,
        [Description("2+")]
        twoplus = 9,
        [Description("4-")]
        fourmin = 10,
        [Description("4+")]
        fourplus = 11,
        [Description("8+")]
        eightplus = 12,
        [Description("C2+")]
        Ctwoplus = 13,
        [Description("C4+")]
        Cfourplus = 14,
    }
}
