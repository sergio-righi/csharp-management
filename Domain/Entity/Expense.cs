using Domain.Base;
using Domain.Entity.Generic;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Tool.Resources;
using Tool.Utilities;

namespace Domain.Entity
{
    public class Expense : BaseEntity
    {
        public Expense()
        {
            this.Date = DateTime.Now.Date;
        }

        public int PersonId { get; set; }
        public int? ReceiptId { get; set; }
        [RequiredResource]
        [DisplayNameResource(nameof(Label.Value))]
        public decimal? Value { get; set; }
        [RequiredResource]
        [DisplayNameResource(nameof(Label.Date))]
        public DateTime? Date { get; set; }
        [RequiredResource]
        [StringLength(255)]
        [DisplayNameResource(nameof(Label.Description))]
        public string Description { get; set; }

        [NotMapped]
        [DisplayNameResource(nameof(Label.Person))]
        public string NotName { get; set; }
        [NotMapped]
        public GFormFile NotReceipt { get; set; }

        public virtual File Receipt { get; set; }
        public virtual Person Person { get; set; }
    }
}
