using System;
using System.Collections.Generic;
using System.Text;
using Tool.Utilities;

namespace Domain.Entity.Specific
{
    public class SAttendancePerson
    {
        public int Id { get; set; }
        public int PersonId { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public string Observation { get; set; }
        public EAttendance SituationId { get; set; }

        public bool IsChecked(EAttendance situationId)
        {
            return SituationId == situationId;
        }
    }
}
