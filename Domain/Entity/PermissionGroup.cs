using System;
using System.Collections.Generic;
using System.Text;
using Domain.Base;
using Tool.Utilities;

namespace Domain.Entity
{
    public class PermissionGroup : BaseGroup
    {
        public int PermissionId { get; set; }
        public bool Add { get; set; }
        public bool Update { get; set; }
        public bool Read { get; set; }
        public bool Print { get; set; }
        public bool IsDeleted { get; set; }

        public virtual Permission Permission { get; set; }
    }
}
