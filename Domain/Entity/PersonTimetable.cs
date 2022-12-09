using Domain.Base;
using System;
using System.Collections.Generic;
using System.Text;
using Tool.Utilities;

namespace Domain.Entity
{
    public class PersonTimetable : BaseEntity
    {
        public int PersonId { get; set; }
        public int TimetableId { get; set; }

        public virtual Person Person { get; set; }
        public virtual Timetable Timetable { get; set; }
    }
}
