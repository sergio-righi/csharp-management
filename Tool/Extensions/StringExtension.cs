using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Tool.Utilities;

namespace Tool.Extensions
{
    public static class StringExtension
    {
        public static bool ContainsCaseInsensitive(this string text, string value)
        {
            return text?.IndexOf(value, StringComparison.CurrentCultureIgnoreCase) >= 0;
        }

        public static bool ContainsIgnoreAccentuation(this string text, string value)
        {
            return text?.RemoveAccentuation().ContainsCaseInsensitive(value.RemoveAccentuation()) ?? false;
        }

        public static string RemoveAccentuation(this string value)
        {
            value = value.Trim();

            if (!string.IsNullOrWhiteSpace(value))
            {
                StringBuilder stringBuilder = new StringBuilder();

                char[] arrayText = value.Normalize(NormalizationForm.FormD).ToCharArray();

                foreach (char letter in arrayText)
                {
                    if (CharUnicodeInfo.GetUnicodeCategory(letter) != UnicodeCategory.NonSpacingMark)
                    {
                        stringBuilder.Append(letter);
                    }
                }

                return stringBuilder.ToString();
            }

            return value;
        }

        public static string RemoveMask(this string value)
        {
            value = value?.Trim();

            if (!string.IsNullOrWhiteSpace(value))
            {
                bool match = Regex.IsMatch(value, @"[^\d]");

                if (match)
                {
                    return string.Join(string.Empty, Regex.Split(value, @"[^\d]"));
                }
            }

            return value;
        }

        public static int RomanToInt(this string roman)
        {
            Dictionary<string, int> equivalency = new Dictionary<string, int>
            {
                { "M", 1000 },
                { "CM", 900 },
                { "D", 500 },
                { "CD", 400 },
                { "C", 100 },
                { "XC", 90 },
                { "L", 50 },
                { "XL", 40 },
                { "X", 10 },
                { "IX", 9 },
                { "V", 5 },
                { "IV", 4 },
                { "I", 1 }
            };

            int number = 0;

            foreach (var item in equivalency)
            {
                while (roman.IndexOf(item.Key.ToString()) == 0)
                {
                    number += int.Parse(item.Value.ToString());

                    roman = roman.Substring(item.Key.ToString().Length);
                }
            }

            return number;
        }

        public static string ToCPF(this string value)
        {
            try
            {
                return Convert.ToUInt64(value).ToString(@"000\.000\.000\-00");
            }
            catch
            {
                return string.Empty;
            }
        }

        public static string ToCNPJ(this string value)
        {
            try
            {
                return Convert.ToUInt64(value).ToString(@"00\.000\.000\/0000\-00");
            }
            catch
            {
                return string.Empty;
            }
        }

        public static string ToCEP(this string value)
        {
            try
            {
                return Convert.ToUInt64(value).ToString(@"00000\-000");
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Only for brazilian numbers (cellphone default)
        /// </summary>
        public static string ToPhone(this string value, EPhone phone = EPhone.Cellphone)
        {
            try
            {
                switch(phone)
                {
                    case EPhone.Residential:
                        return Convert.ToUInt64(value).ToString(@"\(00\) 0000\-0000");
                    default:
                         return Convert.ToUInt64(value).ToString(@"\(00\) 0 0000\-0000");
                }
            }
            catch
            {
                return string.Empty;
            }
        }

        public static string FirstLetter(this string value)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                return value.Substring(0, 1).ToUpper();
            }

            return string.Empty;
        }
    }
}
