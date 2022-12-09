using System;
using System.Collections.Generic;
using System.Text;
using Domain.Base;
using Tool.Utilities;
using Tool.Resources;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entity.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entity
{
    public class Overtime : BaseEntity
    {
        public int? FileId { get; set; }
        [RequiredResource]
        public int PersonId { get; set; }
        [RequiredResource]
        [DisplayNameResource(nameof(Label.Date))]
        public DateTime? Date { get; set; }
        [RequiredResource]
        [DisplayNameResource(nameof(Label.Quantity))]
        public int? Count { get; set; }
        [RequiredResource]
        [DisplayNameResource(nameof(Label.Value))]
        public decimal? Earning { get; set; }
        public DateTime? PaymentDate { get; set; }
        public DateTime? ConfirmationDate { get; set; }
        [DisplayNameResource(nameof(Label.Confirmed))]
        public bool IsConfirmed { get; set; }
        [DisplayNameResource(nameof(Label.Paid))]
        public bool IsPaid { get; set; }

        [NotMapped]
        [RequiredResource]
        [DisplayNameResource(nameof(Label.Employee))]
        public string NotName { get; set; }
        [NotMapped]
        [DisplayNameResource(nameof(Label.File))]
        public GFormFile NotFile { get; set; }

        public virtual File File { get; set; }
        public virtual Person Person { get; set; }
    }
}
