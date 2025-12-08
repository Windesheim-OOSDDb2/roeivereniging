using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoeiVereniging.Core.Helpers
{
    internal class ConvertDateToDayHelper
    {
        private static readonly string[] DutchDayNames = 
        {
            "zondag", "maandag", "dinsdag", "woensdag", "donderdag", "vrijdag", "zaterdag"
        };

        public static string ConvertToDayName(string dateString)
        {
            if (DateTime.TryParseExact(dateString, "dd-MM-yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime date))
            {
                return DutchDayNames[(int)date.DayOfWeek];
            }
            throw new ArgumentException("Invalid date format. Expected format is 'dd-MM-yyyy'.");
        }
    }
}
