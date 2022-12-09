using System;
using System.Collections.Generic;
using System.Text;
using Domain.Base;
using Tool.Utilities;

namespace Domain.Entity
{
    public class PersonRole : BaseEntity
    {
        public int PersonId { get; set; }
        public ERole RoleId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public virtual Person Person { get; set; }
    }
}
