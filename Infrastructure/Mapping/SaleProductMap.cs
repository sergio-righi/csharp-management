using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Mapping
{
    public class SaleProductMap : IEntityTypeConfiguration<SaleProduct>
    {
        public void Configure(EntityTypeBuilder<SaleProduct> builder)
        {
            builder.ToTable(nameof(SaleProduct));

            builder.HasKey(x => x.Id);

            builder.Property(x => x.SaleId).HasColumnName(nameof(SaleProduct.SaleId));
            builder.Property(x => x.ProductId).HasColumnName(nameof(SaleProduct.ProductId));

            builder.Property(x => x.Width).HasColumnName(nameof(SaleProduct.Width));
            builder.Property(x => x.Height).HasColumnName(nameof(SaleProduct.Height));
            builder.Property(x => x.Weight).HasColumnName(nameof(SaleProduct.Weight));
            builder.Property(x => x.Value).HasColumnName(nameof(SaleProduct.Value));
            builder.Property(x => x.Count).HasColumnName(nameof(SaleProduct.Count));

            builder.Property(x => x.CreatedBy).HasColumnName(nameof(SaleProduct.CreatedBy));
            builder.Property(x => x.CreatedAt).HasColumnName(nameof(SaleProduct.CreatedAt));
            builder.Property(x => x.UpdatedBy).HasColumnName(nameof(SaleProduct.UpdatedBy));
            builder.Property(x => x.UpdatedAt).HasColumnName(nameof(SaleProduct.UpdatedAt));

            builder.HasOne(x => x.Sale)
                .WithMany(x => x.Products)
                .HasForeignKey(x => x.SaleId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Product)
                .WithMany(x => x.SaleProducts)
                .HasForeignKey(x => x.ProductId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
