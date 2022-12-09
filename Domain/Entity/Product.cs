using Domain.Base;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Tool.Resources;
using Tool.Utilities;

namespace Domain.Entity
{
    public class Product : BaseEntity
    {
        public int ItemId { get; set; }
        [DisplayNameResource(nameof(Label.Size))]
        public ESize SizeId { get; set; }
        [DisplayNameResource(nameof(Label.Color))]
        public EColor ColorId { get; set; }
        [DisplayNameResource(nameof(Label.Material))]
        public EMaterial MaterialId { get; set; }
        [DisplayNameResource(nameof(Label.Shape))]
        public EShape ShapeId { get; set; }
        [RequiredResource]
        [DisplayNameResource(nameof(Label.Calculation))]
        public ECalculation CalculationId { get; set; }
        [RequiredResource]
        [StringLength(150)]
        [DisplayNameResource(nameof(Label.Name))]
        public string Name { get; set; }
        [StringLength(250)]
        [DisplayNameResource(nameof(Label.Description))]
        public string Description { get; set; }
        [StringLength(50)]
        [DisplayNameResource(nameof(Label.Code))]
        public string Code { get; set; }
        [DisplayNameResource(nameof(Label.Quantity))]
        public int? Count { get; set; }
        [DisplayNameResource(nameof(Label.PurchasePrice))]
        public decimal? PurchasePrice { get; set; }
        [RequiredOneResource(nameof(RentPrice))]
        [DisplayNameResource(nameof(Label.SalePrice))]
        public decimal? SalePrice { get; set; }
        [RequiredOneResource(nameof(SalePrice))]
        [DisplayNameResource(nameof(Label.RentPrice))]
        public decimal? RentPrice { get; set; }
        [DisplayNameResource(nameof(Label.Width))]
        public float? Width { get; set; }
        [DisplayNameResource(nameof(Label.Height))]
        public float? Height { get; set; }
        [DisplayNameResource(nameof(Label.Weight))]
        public float? Weight { get; set; }
        [DisplayNameResource(nameof(Label.Activated))]
        public bool IsActivated { get; set; }
        public bool IsDeleted { get; set; }

        [NotMapped]
        [RequiredResource]
        [DisplayNameResource(nameof(Label.Item))]
        public string NotItemName { get; set; }

        public virtual Item Item { get; set; }

        public virtual ICollection<ProductImage> Images { get; set; }
        public virtual ICollection<RentProduct> RentProducts { get; set; }
        public virtual ICollection<SaleProduct> SaleProducts { get; set; }
    }
}
