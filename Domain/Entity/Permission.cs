using System;
using System.Collections.Generic;
using System.Text;
using Domain.Base;
using Tool.Utilities;

namespace Domain.Entity
{
    public class Permission : BaseEntity
    {
        public int MenuId { get; set; }

        public virtual Menu Menu { get; set; }

        public virtual ICollection<PermissionGroup> Permissions { get; set; }
    }
}
