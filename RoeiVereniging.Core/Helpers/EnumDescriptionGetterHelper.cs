using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoeiVereniging.Core.Helpers
{
    public static class EnumExtensions
    {
        public static string GetEnumDescription(this Enum value)
        {
            var enumField = value.GetType().GetField(value.ToString());
            var attribute = enumField?
                .GetCustomAttributes(typeof(DescriptionAttribute), false)
                .FirstOrDefault() as DescriptionAttribute;

            return attribute?.Description ?? value.ToString();
        }
    }

}
