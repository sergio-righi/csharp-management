using Domain.Base;
using Domain.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using Tool.Utilities;

namespace Domain.Entity
{
    public class Timetable : BaseEntity
    {
        public EShift ShiftId { get; set; }
        public DayOfWeek Weekday { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public bool IsActivated { get; set; }

        public virtual ICollection<AttendanceTimetable> AttendanceTimetable { get; set; }
        public virtual ICollection<PersonTimetable> PersonTimetable { get; set; }
    }
}
