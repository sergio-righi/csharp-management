using System;
using System.Collections.Generic;
using System.Text;
using Domain.Base;
using Tool.Utilities;
using Tool.Resources;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entity.Generic;

namespace Domain.Entity
{
    public class Vacation : BaseEntity
    {
        public int? FileId { get; set; }
        [RequiredResource]
        public int PersonId { get; set; }
        [RequiredResource]
        [DisplayNameResource(nameof(Label.StartDate))]
        [DateCompareResource(nameof(EndDate), Compare.LessThan)]
        public DateTime? StartDate { get; set; }
        [RequiredResource]
        [DisplayNameResource(nameof(Label.EndDate))]
        [DateCompareResource(nameof(StartDate), Compare.MoreThan)]
        public DateTime? EndDate { get; set; }
        [RequiredResource]
        [DisplayNameResource(nameof(Label.Amount))]
        public decimal? Amount { get; set; }
        public DateTime? ConfirmationDate { get; set; }
        [DisplayNameResource(nameof(Label.Confirmed))]
        public bool IsConfirmed { get; set; }

        [NotMapped]
        [RequiredResource]
        [DisplayNameResource(nameof(Label.Employee))]
        public string NotName { get; set; }
        [NotMapped]
        public GFormFile NotVoucher { get; set; }

        public virtual File File { get; set; }
        public virtual Person Person { get; set; }
    }
}
