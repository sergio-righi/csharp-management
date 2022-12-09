using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Mapping
{
    public class ProductImageMap : IEntityTypeConfiguration<ProductImage>
    {
        public void Configure(EntityTypeBuilder<ProductImage> builder)
        {
            builder.ToTable(nameof(ProductImage));

            builder.HasKey(x => x.Id);

            builder.Property(x => x.ImageId).HasColumnName(nameof(ProductImage.ImageId));
            builder.Property(x => x.ProductId).HasColumnName(nameof(ProductImage.ProductId));

            builder.Property(x => x.IsDeleted).HasColumnName(nameof(ProductImage.IsDeleted));

            builder.Property(x => x.CreatedBy).HasColumnName(nameof(ProductImage.CreatedBy));
            builder.Property(x => x.CreatedAt).HasColumnName(nameof(ProductImage.CreatedAt));
            builder.Property(x => x.UpdatedBy).HasColumnName(nameof(ProductImage.UpdatedBy));
            builder.Property(x => x.UpdatedAt).HasColumnName(nameof(ProductImage.UpdatedAt));

            builder.HasOne(x => x.Image)
                .WithMany(x => x.Images)
                .HasForeignKey(x => x.ImageId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Product)
                .WithMany(x => x.Images)
                .HasForeignKey(x => x.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
