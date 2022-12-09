using System;
using System.Collections.Generic;
using System.Text;
using Tool.Resources;

namespace Tool.Extensions
{
    public static class DateTimeExtension
    {
        public static DateTime GetNextWorkday(this DateTime datetime)
        {
            if (datetime.DayOfWeek == DayOfWeek.Saturday)
            {
                return datetime.AddDays(2);
            }
            else if (datetime.DayOfWeek == DayOfWeek.Sunday)
            {
                return datetime.AddDays(1);
            }

            return datetime;
        }

        public static DateTime AddWorkday(this DateTime datetime, int value)
        {
            for (int i = 0; i < value; i++)
            {
                datetime = datetime.AddDays(1).GetNextWorkday();
            }

            return datetime;
        }

        public static bool IsWorkday(this DateTime datetime)
        {
            return (datetime.DayOfWeek != DayOfWeek.Saturday && datetime.DayOfWeek != DayOfWeek.Sunday);
        }

        public static string GetRelativeDate(this DateTime datetime, bool date = true)
        {
            TimeSpan timeSpan = DateTime.Now.Subtract(datetime);

            if (timeSpan <= TimeSpan.FromSeconds(60))
            {
                return date ? Label.Today : $"{timeSpan.Seconds} seconds ago";
            }
            else if (timeSpan <= TimeSpan.FromMinutes(60))
            {
                return date ? Label.Today : (timeSpan.Minutes > 1 ? $"{timeSpan.Minutes} minutes ago" : "one minute ago");
            }
            else if (timeSpan <= TimeSpan.FromHours(24))
            {
                return date ? Label.Today : (timeSpan.Hours > 1 ? $"{timeSpan.Hours} hours ago" : "one hour ago");
            }
            else if (timeSpan <= TimeSpan.FromDays(30))
            {
                return timeSpan.Days > 1 ? $"{timeSpan.Days} days ago" : Label.Yesterday;
            }
            else if (timeSpan <= TimeSpan.FromDays(365))
            {
                return timeSpan.Days > 30 ? $"{timeSpan.Days / 30} months ago" : "one month ago";
            }
            else
            {
                return timeSpan.Days > 365 ? $"{timeSpan.Days / 365} years ago" : "one year ago";
            }
        }

        public static int GetSemester(this DateTime datetime)
        {
            return datetime.Month > 6 ? 2 : 1;
        }

        public static DateTime GetDate(this DateTime datetime)
        {
            return new DateTime(datetime.Year, datetime.Month, datetime.Day, 0, 0, 0);
        }
    }
}
