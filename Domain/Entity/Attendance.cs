using Domain.Base;
using Domain.Entity.Generic;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Tool.Resources;
using Tool.Utilities;

namespace Domain.Entity
{
    public class Attendance : BaseEntity
    {
        public int PersonId { get; set; }
        public int? FileId { get; set; }
        [RequiredResource]
        [DisplayNameResource(nameof(Label.Date))]
        public DateTime Date { get; set; }
        [DisplayNameResource(nameof(Label.Observation))]
        public string Observation { get; set; }

        [NotMapped]
        public GFormFile NotFile { get; set; }

        public virtual File File { get; set; }
        public virtual Person Person { get; set; }
        
        public virtual ICollection<AttendanceTimetable> AttendanceTimetable { get; set; }
    }
}
