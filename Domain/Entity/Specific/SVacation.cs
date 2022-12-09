using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entity.Specific
{
    public class SVacation
    {
        public int Id { get; set; }
        public int? FileId { get; set; }
        public int? PersonId { get; set; }
        public string Name { get; set; }
        public int? Year { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsConfirmed { get; set; }
    }
}
