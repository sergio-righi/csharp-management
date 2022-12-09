using Domain.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Tool.Utilities;
using Tool.Resources;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entity.Generic;

namespace Domain.Entity
{
    public class Paycheck : BaseEntity
    {
        public Paycheck()
        {
            this.PaymentDate = DateTime.Now.Date;
        }

        public int? FileId { get; set; }
        [RequiredResource]
        public int PayrollId { get; set; }
        [RequiredResource]
        public int PersonId { get; set; }
        [RequiredResource]
        [DisplayNameResource(nameof(Label.RegularHour))]
        public int? RegularHour { get; set; }
        [DisplayNameResource(nameof(Label.Week))]
        public int? Week { get; set; }
        [RequiredResource]
        [DisplayNameResource(nameof(Label.HourRate))]
        public decimal? HourRate { get; set; }
        [RequiredResource]
        [DisplayNameResource(nameof(Label.Overtime))]
        public int? OvertimeCount { get; set; }
        [RequiredResource]
        [DisplayNameResource(nameof(Label.OvertimeValue))]
        public decimal? OvertimeValue { get; set; }
        [RequiredResource]
        [DisplayNameResource(nameof(Label.GrossEarning))]
        public decimal? GrossEarning { get; set; }
        [DisplayNameResource(nameof(Label.Bonus))]
        public decimal? Bonus { get; set; }
        [DisplayNameResource(nameof(Label.Tax))]
        public decimal? Tax { get; set; }
        [DisplayNameResource(nameof(Label.Deduction))]
        public decimal? Deduction { get; set; }
        [DisplayNameResource(nameof(Label.Observation))]
        public string Observation { get; set; }
        [RequiredResource]
        [DisplayNameResource(nameof(Label.PaymentDate))]
        public DateTime PaymentDate { get; set; }
        public DateTime? ConfirmationDate { get; set; }
        [DisplayNameResource(nameof(Label.Extra))]
        public bool IsExtra { get; set; }
        [DisplayNameResource(nameof(Label.Confirmed))]
        public bool IsConfirmed { get; set; }

        [NotMapped]
        public GFormFile NotVoucher { get; set; }
        [NotMapped]
        [DisplayNameResource(nameof(Label.Employee))]
        public string NotName { get; set; }

        public virtual File File { get; set; }
        public virtual Person Person { get; set; }
        public virtual Payroll Payroll { get; set; }
    }
}
