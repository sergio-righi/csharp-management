using Domain.Entity;
using Domain.Entity.Specific;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tool.Utilities;

namespace Application.Models.Attendance
{
    public class AttendancePeopleModel
    {
        public DateTime date { get; set; }
        public ICollection<SAttendancePerson> list { get; set; }
    }
}
