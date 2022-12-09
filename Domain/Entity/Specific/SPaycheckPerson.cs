using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entity.Specific
{
    public class SPaycheckPerson
    {
        public int Id { get; set; }
        public int PersonId { get; set; }
        public string Name { get; set; }
        public int? Week { get; set; }
        public int? VoucherId { get; set; }
        public bool IsExtra { get; set; }
    }
}
