using Application.Classes;
using Domain.Entity;
using System;
using System.Collections.Generic;
using Tool.Utilities;

namespace Application.Models.Attendance
{
    public class AttendanceFilter
    {
        public int month { get; set; }
        public int year { get; set; }
        public IEnumerable<DateTime> list { get; set; }

        public AttendanceFilter()
        {
            year = DateTime.Now.Year;

            month = DateTime.Now.Month;
        }
    }
}