using Domain.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Tool.Utilities;
using Tool.Resources;

namespace Domain.Entity
{
    public class Bill : BaseEntity
    {
        [RequiredResource]
        [DisplayNameResource(nameof(Label.Type))]
        public EBill TypeId { get; set; }
        [RequiredResource]
        public ECurrency CurrencyId { get; set; }
        public int? BillId { get; set; }
        public int? ReceiptId { get; set; }
        public int? PersonId { get; set; }
        public int? InstallmentId { get; set; }
        [MaxLengthResource(255)]
        [DisplayNameResource(nameof(Label.Description))]
        public string Description { get; set; }
        [RequiredResource]
        [DisplayNameResource(nameof(Label.DueDate))]
        public DateTime? DueDate { get; set; }
        [DisplayNameResource(nameof(Label.PaymentDate))]
        public DateTime? PaymentDate { get; set; }
        [DisplayNameResource(nameof(Label.LimitDate))]
        public DateTime? LimitDate { get; set; }
        [RequiredResource]
        [DisplayNameResource(nameof(Label.Value))]
        public decimal? Value { get; set; }
        [DisplayNameResource(nameof(Label.InterestValue))]
        public decimal? InterestValue { get; set; }
        [DisplayNameResource(nameof(Label.InterestRate))]
        public float? InterestRate { get; set; }
        [RequiredResource]
        [DisplayNameResource(nameof(Label.InstallmentCount))]
        public int? InstallmentCount { get; set; }
        [DisplayNameResource(nameof(Label.Paid))]
        public bool IsPaid { get; set; }

        [NotMapped]
        [DisplayNameResource(nameof(Label.Related))]
        public string NotName { get; set; }
        [NotMapped]
        [DisplayNameResource(nameof(Label.Bill))]
        public string NotBillName { get; set; }
        [NotMapped]
        [DisplayNameResource(nameof(Label.Receipt))]
        public string NotReceiptName { get; set; }

        public virtual File File { get; set; }
        public virtual File Receipt { get; set; }
        public virtual Person Person { get; set; }
        public virtual Installment Installment { get; set; }
    }
}
