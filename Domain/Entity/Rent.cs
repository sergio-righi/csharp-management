using Domain.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Tool.Utilities;
using Tool.Resources;

namespace Domain.Entity
{
    public class Rent : BaseEntity
    {
        public Rent()
        {
            this.StartDate = DateTime.Now.Date;
        }

        [RequiredResource]
        [DisplayNameResource(nameof(Label.Status))]
        public ERent SituationId { get; set; }
        public int EmployeeId { get; set; }
        public int CustomerId { get; set; }
        [RequiredResource]
        [DisplayNameResource(nameof(Label.Address))]
        public int AddressId { get; set; }
        public int? InstallmentId { get; set; }
        [DisplayNameResource(nameof(Label.Shipping))]
        public decimal? ShippingFee { get; set; }
        [RequiredResource]
        [DisplayNameResource(nameof(Label.PaymentMethod))]
        public EPaymentMethod PaymentMethodId { get; set; }
        [DisplayNameResource(nameof(Label.DownPayment))]
        public decimal? DownPayment { get; set; }
        [RequiredResource]
        [DisplayNameResource(nameof(Label.InstallmentCount))]
        public int? InstallmentCount { get; set; }
        [DisplayNameResource(nameof(Label.InterestRate))]
        public float? InterestRate { get; set; }
        [RequiredResource]
        [DisplayNameResource(nameof(Label.StartDate))]
        [DateCompareResource(nameof(EndDate), Compare.LessOrEquals)]
        public DateTime? StartDate { get; set; }
        [RequiredResource]
        [DisplayNameResource(nameof(Label.EndDate))]
        [DateCompareResource(nameof(StartDate), Compare.MoreOrEquals)]
        public DateTime? EndDate { get; set; }
        public bool IsDeleted { get; set; }

        [NotMapped]
        [RequiredResource]
        [DisplayNameResource(nameof(Label.Customer))]
        public string NotCustomerName { get; set; }

        public virtual Person Employee { get; set; }
        public virtual Person Customer { get; set; }
        public virtual Address Address { get; set; }
        public virtual Installment Installment { get; set; }

        public virtual ICollection<RentProduct> Products { get; set; }
    }
}
