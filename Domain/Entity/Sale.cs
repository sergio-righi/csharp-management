using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Domain.Base;
using Tool.Utilities;
using Tool.Resources;

namespace Domain.Entity
{
    public class Sale : BaseEntity
    {
        public Sale()
        {
            this.Date = DateTime.Now.Date;
        }

        [RequiredResource]
        [DisplayNameResource(nameof(Label.Status))]
        public ESale SituationId { get; set; }
        public int SellerId { get; set; }
        public int CustomerId { get; set; }
        public int? VendorId { get; set; }
        [RequiredResource]
        [DisplayNameResource(nameof(Label.Address))]
        public int? AddressId { get; set; }
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
        [MaxLengthResource(255)]
        [DisplayNameResource(nameof(Label.Description))]
        public string Description { get; set; }
        [RequiredResource]
        [CurrentDateResource(Compare.LessOrEquals)]
        [DisplayNameResource(nameof(Label.Date))]
        public DateTime? Date { get; set; }
        public bool IsDeleted { get; set; }

        [NotMapped]
        [RequiredResource]
        [DisplayNameResource(nameof(Label.Customer))]
        public string NotCustomerName { get; set; }
        [NotMapped]
        [DisplayNameResource(nameof(Label.Vendor))]
        public string NotVendorName { get; set; }

        public virtual Person Seller { get; set; }
        public virtual Person Vendor { get; set; }
        public virtual Person Customer { get; set; }
        public virtual Address Address { get; set; }
        public virtual Installment Installment { get; set; }

        public virtual ICollection<SaleProduct> Products { get; set; }
    }
}
