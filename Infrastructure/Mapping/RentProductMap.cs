using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Mapping
{
    public class RentalProductMap : IEntityTypeConfiguration<RentProduct>
    {
        public void Configure(EntityTypeBuilder<RentProduct> builder)
        {
            builder.ToTable(nameof(RentProduct));

            builder.HasKey(x => x.Id);

            builder.Property(x => x.RentId).HasColumnName(nameof(RentProduct.RentId));
            builder.Property(x => x.ProductId).HasColumnName(nameof(RentProduct.ProductId));

            builder.Property(x => x.Count).HasColumnName(nameof(RentProduct.Count));
            builder.Property(x => x.Value).HasColumnName(nameof(RentProduct.Value));

            builder.Property(x => x.CreatedBy).HasColumnName(nameof(RentProduct.CreatedBy));
            builder.Property(x => x.CreatedAt).HasColumnName(nameof(RentProduct.CreatedAt));
            builder.Property(x => x.UpdatedBy).HasColumnName(nameof(RentProduct.UpdatedBy));
            builder.Property(x => x.UpdatedAt).HasColumnName(nameof(RentProduct.UpdatedAt));

            builder.HasOne(x => x.Rent)
                .WithMany(x => x.Products)
                .HasForeignKey(x => x.RentId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Product)
                .WithMany(x => x.RentProducts)
                .HasForeignKey(x => x.ProductId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
