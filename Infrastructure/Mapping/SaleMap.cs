using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Mapping
{
    public class SaleMap : IEntityTypeConfiguration<Sale>
    {
        public void Configure(EntityTypeBuilder<Sale> builder)
        {
            builder.ToTable(nameof(Sale));

            builder.HasKey(x => x.Id);

            builder.Property(x => x.SellerId).HasColumnName(nameof(Sale.SellerId));
            builder.Property(x => x.CustomerId).HasColumnName(nameof(Sale.CustomerId));
            builder.Property(x => x.VendorId).HasColumnName(nameof(Sale.VendorId));
            builder.Property(x => x.SituationId).HasColumnName(nameof(Sale.SituationId));
            builder.Property(x => x.PaymentMethodId).HasColumnName(nameof(Sale.PaymentMethodId));
            builder.Property(x => x.AddressId).HasColumnName(nameof(Sale.AddressId));

            builder.Property(x => x.ShippingFee).HasColumnName(nameof(Sale.ShippingFee));
            builder.Property(x => x.DownPayment).HasColumnName(nameof(Sale.DownPayment));
            builder.Property(x => x.InstallmentCount).HasColumnName(nameof(Sale.InstallmentCount));
            builder.Property(x => x.InterestRate).HasColumnName(nameof(Sale.InterestRate));
            builder.Property(x => x.Description).HasColumnName(nameof(Sale.Description));
            builder.Property(x => x.Date).HasColumnName(nameof(Sale.Date));
            builder.Property(x => x.IsDeleted).HasColumnName(nameof(Sale.IsDeleted));

            builder.Property(x => x.CreatedBy).HasColumnName(nameof(Sale.CreatedBy));
            builder.Property(x => x.CreatedAt).HasColumnName(nameof(Sale.CreatedAt));
            builder.Property(x => x.UpdatedBy).HasColumnName(nameof(Sale.UpdatedBy));
            builder.Property(x => x.UpdatedAt).HasColumnName(nameof(Sale.UpdatedAt));

            builder.HasOne(x => x.Seller)
                .WithMany(x => x.Sales)
                .HasForeignKey(x => x.SellerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Customer)
                .WithMany(x => x.Purchases)
                .HasForeignKey(x => x.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Vendor)
                .WithMany(x => x.Providers)
                .HasForeignKey(x => x.VendorId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired(false);

            builder.HasOne(x => x.Address)
                .WithMany(x => x.Sales)
                .HasForeignKey(x => x.AddressId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Installment)
                .WithOne(x => x.Sale)
                .HasForeignKey<Sale>(x => x.InstallmentId)
                .OnDelete(DeleteBehavior.SetNull)
                .IsRequired(false);
        }
    }
}
