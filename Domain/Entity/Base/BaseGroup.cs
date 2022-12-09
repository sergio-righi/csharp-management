using Domain.Entity;
using Domain.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using Tool.Utilities;

namespace Domain.Base
{
    public abstract class BaseGroup : BaseEntity, IEntity
    {
        public int? PersonId { get; set; }
        public ERole RoleId { get; set; }

        public virtual Person Person { get; set; }
    }
}
