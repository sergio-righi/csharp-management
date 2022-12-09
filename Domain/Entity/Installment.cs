using Domain.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entity
{
    public class Installment : BaseEntity
    {
        public virtual Bill Bill { get; set; }
        public virtual Rent Rent { get; set; }
        public virtual Sale Sale { get; set; }
        public virtual ICollection<InstallmentInfo> Dates { get; set; }
    }
}
