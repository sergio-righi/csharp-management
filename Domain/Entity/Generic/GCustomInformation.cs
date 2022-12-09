using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entity.Generic
{
    public class GCustomInformation
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime? Date { get; set; }
        public string Description { get; set; }
    }
}
