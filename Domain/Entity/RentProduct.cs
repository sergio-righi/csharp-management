using System;
using System.Collections.Generic;
using System.Text;
using Domain.Base;
using Tool.Utilities;

namespace Domain.Entity
{
    public class RentProduct : BaseEntity
    {
        public int RentId { get; set; }
        public int ProductId { get; set; }
        public decimal? Value { get; set; }
        public int Count { get; set; }

        public virtual Rent Rent { get; set; }
        public virtual Product Product { get; set; }
    }
}
