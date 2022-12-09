using Domain.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Base
{
    public abstract class BaseFriendly : BaseEntity, IEntity
    {
        public virtual string Friendly_id { get; set; }
    }
}
