using Domain.Base;
using System;
using System.Collections.Generic;
using System.Text;
using Tool.Utilities;

namespace Domain.Entity
{
    public class AttendanceTimetable : BaseEntity
    {
        public int AttendanceId { get; set; }
        public int TimetableId { get; set; }
        public EAttendance SituationId { get; set; }
        public float Percentage { get; set; }

        public virtual Timetable Timetable { get; set; }
        public virtual Attendance Attendance { get; set; }
    }
}
