using Domain.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entity
{
    public class NotificationGroup : BaseEntity
    {
        public int PersonId { get; set; }
        public int NotificationId { get; set; }
        public bool IsVisualized { get; set; }

        public virtual Person Person { get; set; }
        public virtual Notification Notification { get; set; }
    }
}
