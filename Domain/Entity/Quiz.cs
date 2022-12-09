using System;
using System.Collections.Generic;
using System.Text;
using Domain.Base;

namespace Domain.Entity
{
    public class Quiz : BaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsDeleted { get; set; }
    }
}
