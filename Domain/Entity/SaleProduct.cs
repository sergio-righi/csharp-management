using Domain.Base;
using System;
using System.Collections.Generic;
using System.Text;
using Tool.Utilities;

namespace Domain.Entity
{
    public class SaleProduct : BaseEntity
    {
        public int SaleId { get; set; }
        public int ProductId { get; set; }
        public float? Width { get; set; }
        public float? Height { get; set; }
        public float? Weight { get; set; }
        public decimal? Value { get; set; }
        public int Count { get; set; }

        public virtual Sale Sale { get; set; }
        public virtual Product Product { get; set; }
    }
}
