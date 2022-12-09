using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Tool.Extensions;
using System.Text.RegularExpressions;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Tool.Utilities
{
    public static class Util
    {
        public static bool ValidateCPFOrCNPJ(string value)
        {
            #region IMPLEMENTAÇÃO

            if (string.IsNullOrWhiteSpace(value))
            {
                return false;
            }

            int j, i, sum;
            int[] digits = new int[2];
            int[] numbers = new int[14];
            string sequence, onlyNumbers;

            onlyNumbers = Regex.Replace(value.RemoveMask(), "[^0-9]", string.Empty);

            if (onlyNumbers.Length == 0 || new string(onlyNumbers[0], onlyNumbers.Length) == onlyNumbers)
            {
                return false;
            }

            if (onlyNumbers.Length == 11)
            {
                for (i = 0; i <= 10; i++)
                {
                    numbers[i] = Convert.ToInt32(onlyNumbers.Substring(i, 1));
                }

                for (i = 0; i <= 1; i++)
                {
                    sum = 0;

                    for (j = 0; j <= 8 + i; j++)
                    {
                        sum += numbers[j] * (10 + i - j);
                    }

                    digits[i] = (sum * 10) % 11;

                    if (digits[i] == 10)
                    {
                        digits[i] = 0;
                    }
                }

                return (digits[0] == numbers[9] & digits[1] == numbers[10]);
            }

            if (onlyNumbers.Length == 14)
            {
                sequence = "6543298765432";

                for (i = 0; i <= 13; i++)
                {
                    numbers[i] = Convert.ToInt32(onlyNumbers.Substring(i, 1));
                }

                for (i = 0; i <= 1; i++)
                {
                    sum = 0;

                    for (j = 0; j <= 11 + i; j++)
                    {
                        sum += numbers[j] * Convert.ToInt32(sequence.Substring(j + 1 - i, 1));
                    }

                    digits[i] = (sum * 10) % 11;

                    if (digits[i] == 10)
                    {
                        digits[i] = 0;
                    }
                }

                return (digits[0] == numbers[12] & digits[1] == numbers[13]);
            }

            return false;

            #endregion
        }

        public static bool ValidateDateFormat(string date)
        {
            if (!string.IsNullOrWhiteSpace(date))
            {
                return DateTime.TryParseExact(date, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _);
            }

            return false;
        }

        public static string[] GetWeekdays(bool abbreviated = false)
        {
            if (abbreviated)
            {
                return DateTimeFormatInfo.CurrentInfo.AbbreviatedDayNames;
            }

            return DateTimeFormatInfo.CurrentInfo.DayNames;
        }

        public static string GetDayDescription(int day, bool abbreviated = false)
        {
            return GetWeekdays(abbreviated)[day] ?? string.Empty;
        }

        public static string[] GetMonths(bool abbreviated = false)
        {
            if (abbreviated)
            {
                return DateTimeFormatInfo.CurrentInfo.AbbreviatedMonthNames;
            }

            return DateTimeFormatInfo.CurrentInfo.MonthNames;
        }

        public static string GetMonthDescription(int month, bool abbreviated = false)
        {
            return GetMonths(abbreviated)[month - 1] ?? string.Empty;
        }

        public static bool IsPhone(string value)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                return new Regex(@"^1\d\d(\d\d)?$|^0800 ?\d{3} ?\d{4}$|^(\(0?([1-9a-zA-Z][0-9a-zA-Z])?[1-9]\d\) ?|0?([1-9a-zA-Z][0-9a-zA-Z])?[1-9]\d[ .-]?)?(9|9[ .-])?[2-9]\d{3}[ .-]?\d{4}$").IsMatch(value);
            }

            return false;
        }

        public static bool IsNumber(string value)
        {
            bool number = false;

            char[] characters = value.ToCharArray();

            foreach (var character in characters)
            {
                number = number ? char.IsDigit(character) : number;
            }

            return number;
        }

        public static bool IsValidPassword(string value)
        {
            if (value.Length < 8 || value.Length > 12)
            {
                return false;
            }

            return Regex.IsMatch(value, $"(?=.*\\d)(?=.*[A-Z])(?=.*\\W)");
        }

        public static IConfigurationRoot GetConfiguration(string project, string file = "appsettings.json")
        {
#if DEBUG
            var path = $"{Directory.GetParent(Directory.GetCurrentDirectory())}\\{project}";
#else
var path = $"{Directory.GetCurrentDirectory()}\\";
#endif

            return new ConfigurationBuilder()
                .SetBasePath(path)
                .AddJsonFile(file)
                .AddEnvironmentVariables()
                .Build();
        }

        public static bool VerifyMinimumAge(DateTime date, int minimumAge = 18)
        {
            int age = DateTime.Now.Year - date.Year;

            if (DateTime.Now.Month < date.Month || (DateTime.Now.Month == date.Month && DateTime.Now.Day < date.Day))
            {
                age--;
            }

            return age > minimumAge;
        }

        public static string GetFriendlyId(string value)
        {
            List<string> list = new List<string>();

            value = value.RemoveAccentuation();
            value = Regex.Replace(value, "[^0-9a-zA-Z ]+", string.Empty);

            foreach (var word in value.Split(' '))
            {
                if (word != string.Empty)
                {
                    list.Add(word.ToLower());
                }
            }

            return string.Join("-", list.ToArray());
        }

        public static int[][] GetGame(int[] initialize, int min = 1, int max = 60, int quantity = 6, int game = 2)
        {
            int[][] lottery = new int[game][];

            for (int i = 0; i < game; i++)
            {
                lottery[i] = new int[quantity];

                if (initialize.Length > 0)
                {
                    for (int k = 0; k < initialize.Length && k < quantity;)
                    {
                        lottery[i][k] = initialize[k];
                    }
                }

                for (int j = initialize.Length; j < quantity;)
                {
                    int random = new Random().Next(min, max);

                    if (Array.IndexOf(lottery[i], random) == -1)
                    {
                        lottery[i][j] = random;
                        j++;
                    }
                }

                Array.Sort(lottery[i]);
            }

            return lottery;
        }

        public static decimal GetCalculation(ECalculation calculationId, decimal? value, float? width, float? height, decimal? price, int count)
        {
            if (!value.HasValue)
            {
                price ??= 0;

                switch (calculationId)
                {
                    case ECalculation.Unit:
                        return ((decimal)price) * count;
                    case ECalculation.SquareMeter:
                        return (Convert.ToDecimal(width ?? 1 * height ?? 1) * ((decimal)price) * count) / 100;
                    case ECalculation.Meter:
                        return (Convert.ToDecimal(width ?? 1) * ((decimal)price) * count) / 100;
                }
            }

            return (decimal)value;
        }

        public static bool IsEmployee(ERole roleId)
        {
            return EmployeeRoles.Contains(roleId);
        }

        public static ERole[] EmployeeRoles =>
            new ERole[] {
                ERole.Administrative,
                ERole.CustomerService,
                ERole.Distribuition,
                ERole.Finance,
                ERole.HumanResource,
                ERole.Legal,
                ERole.Marketing,
                ERole.Operations,
                ERole.Production,
                ERole.Purchasing,
                ERole.Research,
                ERole.Sales
            };


        //public ActionResult ConsultarCEP(string cep)
        //{
        //    WSCorreios.enderecoERP enderecoERP = null;

        //    if (cep != null)
        //    {
        //        cep = Utilitario.RemoverCaracteres(cep);

        //        try
        //        {
        //            WSCorreios.AtendeClienteClient webTreatment = new WSCorreios.AtendeClienteClient();

        //            enderecoERP = webTreatment.consultaCEP(cep);
        //        }
        //        catch (Exception ex)
        //        {
        //        }
        //    }

        //    return Json(enderecoERP, JsonRequestBehavior.AllowGet);
        //}
    }
}
