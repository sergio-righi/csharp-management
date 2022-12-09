using Domain.Base;
using System;
using System.Collections.Generic;
using System.Text;
using Tool.Utilities;
using Tool.Resources;

namespace Domain.Entity
{
    public class Payroll : BaseEntity
    {
        [RequiredResource]
        [DisplayNameResource(nameof(Label.Year))]
        public int? Year { get; set; }
        [RequiredResource]
        [DisplayNameResource(nameof(Label.Month))]
        public EMonth Month { get; set; }

        public virtual ICollection<Paycheck> Paychecks { get; set; }
    }
}
