using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entity.Specific
{
    public class SOvertime
    {
        public int Id { get; set; }
        public int? PersonId { get; set; }
        public string Name { get; set; }
        public DateTime? Date { get; set; }
        public int? Count { get; set; }
        public decimal? Earning { get; set; }
        public DateTime? PaymentDate { get; set; }
        public DateTime? ConfirmationDate { get; set; }
        public bool IsConfirmed { get; set; }
        public bool IsPaid { get; set; }
    }
}
