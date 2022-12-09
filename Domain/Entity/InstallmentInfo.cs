using Domain.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Tool.Extensions;

namespace Domain.Entity
{
    public class InstallmentInfo : BaseEntity
    {
        public int InstallmentId { get; set; }
        public int Number { get; set; }
        public DateTime Date { get; set; }
        public DateTime ConfirmationDate { get; set; }
        public bool IsConfirmed { get; set; }

        [NotMapped]
        public bool NotIsDeleted { get; set; }

        public virtual Installment Installment { get; set; }
    }
}
