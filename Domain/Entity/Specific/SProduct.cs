using System;
using System.Collections.Generic;
using System.Text;
using Tool.Utilities;

namespace Domain.Entity.Specific
{
    public class SProduct
    {
        public int Id { get; set; }
        public int RentId { get; set; }
        public int SaleId { get; set; }
        public string Key { get; set; } = string.Empty;
        public string Name { get; set; }
        public int ProductId { get; set; }
        public ECalculation CalculationId { get; set; }
        public int Count { get; set; }
        public decimal? Price { get; set; }
        public float? Width { get; set; }
        public float? Height { get; set; }
        public float? Weight { get; set; }
        public decimal? Value { get; set; }
        public bool IsBlocked { get; set; }
        public string Deleted { get; set; }

        public bool IsDeleted()
        {
            return !string.IsNullOrEmpty(Deleted) ? bool.Parse(Deleted) : false;
        }
    }
}
