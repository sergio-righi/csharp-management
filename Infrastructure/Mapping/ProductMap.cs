using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Mapping
{
    public class ProductMap : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable(nameof(Product));

            builder.HasKey(x => x.Id);

            builder.Property(x => x.ItemId).HasColumnName(nameof(Product.ItemId));
            builder.Property(x => x.SizeId).HasColumnName(nameof(Product.SizeId));
            builder.Property(x => x.ColorId).HasColumnName(nameof(Product.ColorId));
            builder.Property(x => x.MaterialId).HasColumnName(nameof(Product.MaterialId));
            builder.Property(x => x.ShapeId).HasColumnName(nameof(Product.ShapeId));
            builder.Property(x => x.CalculationId).HasColumnName(nameof(Product.CalculationId));

            builder.Property(x => x.Name).HasColumnName(nameof(Product.Name));
            builder.Property(x => x.Description).HasColumnName(nameof(Product.Description));
            builder.Property(x => x.Code).HasColumnName(nameof(Product.Code));
            builder.Property(x => x.Count).HasColumnName(nameof(Product.Count));
            builder.Property(x => x.PurchasePrice).HasColumnName(nameof(Product.PurchasePrice));
            builder.Property(x => x.SalePrice).HasColumnName(nameof(Product.SalePrice));
            builder.Property(x => x.RentPrice).HasColumnName(nameof(Product.RentPrice));
            builder.Property(x => x.Width).HasColumnName(nameof(Product.Width));
            builder.Property(x => x.Height).HasColumnName(nameof(Product.Height));
            builder.Property(x => x.Weight).HasColumnName(nameof(Product.Weight));
            builder.Property(x => x.IsActivated).HasColumnName(nameof(Product.IsActivated));
            builder.Property(x => x.IsDeleted).HasColumnName(nameof(Product.IsDeleted));

            builder.Property(x => x.CreatedBy).HasColumnName(nameof(Product.CreatedBy));
            builder.Property(x => x.CreatedAt).HasColumnName(nameof(Product.CreatedAt));
            builder.Property(x => x.UpdatedBy).HasColumnName(nameof(Product.UpdatedBy));
            builder.Property(x => x.UpdatedAt).HasColumnName(nameof(Product.UpdatedAt));

            builder.HasOne(x => x.Item)
                .WithMany(x => x.Products)
                .HasForeignKey(x => x.ItemId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
