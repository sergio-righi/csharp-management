using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Mapping
{
    public class RentMap : IEntityTypeConfiguration<Rent>
    {
        public void Configure(EntityTypeBuilder<Rent> builder)
        {
            builder.ToTable(nameof(Rent));

            builder.HasKey(x => x.Id);

            builder.Property(x => x.EmployeeId).HasColumnName(nameof(Rent.EmployeeId));
            builder.Property(x => x.CustomerId).HasColumnName(nameof(Rent.CustomerId));
            builder.Property(x => x.SituationId).HasColumnName(nameof(Rent.SituationId));
            builder.Property(x => x.PaymentMethodId).HasColumnName(nameof(Rent.PaymentMethodId));
            builder.Property(x => x.AddressId).HasColumnName(nameof(Rent.AddressId));

            builder.Property(x => x.ShippingFee).HasColumnName(nameof(Rent.ShippingFee));
            builder.Property(x => x.DownPayment).HasColumnName(nameof(Rent.DownPayment));
            builder.Property(x => x.InstallmentCount).HasColumnName(nameof(Rent.InstallmentCount));
            builder.Property(x => x.InterestRate).HasColumnName(nameof(Rent.InterestRate));
            builder.Property(x => x.StartDate).HasColumnName(nameof(Rent.StartDate));
            builder.Property(x => x.EndDate).HasColumnName(nameof(Rent.EndDate));
            builder.Property(x => x.IsDeleted).HasColumnName(nameof(Rent.IsDeleted));

            builder.Property(x => x.CreatedBy).HasColumnName(nameof(Rent.CreatedBy));
            builder.Property(x => x.CreatedAt).HasColumnName(nameof(Rent.CreatedAt));
            builder.Property(x => x.UpdatedBy).HasColumnName(nameof(Rent.UpdatedBy));
            builder.Property(x => x.UpdatedAt).HasColumnName(nameof(Rent.UpdatedAt));

            builder.HasOne(x => x.Employee)
                .WithMany(x => x.EmployeeRents)
                .HasForeignKey(x => x.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Customer)
                .WithMany(x => x.ClientRents)
                .HasForeignKey(x => x.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Address)
                .WithMany(x => x.Rents)
                .HasForeignKey(x => x.AddressId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Installment)
                .WithOne(x => x.Rent)
                .HasForeignKey<Rent>(x => x.InstallmentId)
                .OnDelete(DeleteBehavior.SetNull)
                .IsRequired(false);
        }
    }
}
