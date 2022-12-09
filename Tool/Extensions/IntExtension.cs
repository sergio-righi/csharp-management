using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tool.Extensions
{
    public static class IntExtension
    {
        public static bool IsPositive(this int value)
        {
            return value > 0;
        }

        public static bool IsNegative(this int value)
        {
            return value < 0;
        }

        public static bool IsZero(this int value)
        {
            return value == 0;
        }

        public static bool IsNull(this int? value)
        {
            return !value.HasValue;
        }

        public static bool IsPositive(this int? value)
        {
            return value.HasValue && value.Value > 0;
        }

        public static bool IsNegative(this int? value)
        {
            return value.HasValue && value.Value < 0;
        }

        public static bool IsNegativeOrZero(this int? value)
        {
            return value.HasValue && (value.Value < 0 || value.Value == 0);
        }
    }
}
