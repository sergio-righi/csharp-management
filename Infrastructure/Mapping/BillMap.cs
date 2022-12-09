using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Mapping
{
    public class BillMap : IEntityTypeConfiguration<Bill>
    {
        public void Configure(EntityTypeBuilder<Bill> builder)
        {
            builder.ToTable(nameof(Bill));

            builder.HasKey(x => x.Id);

            builder.Property(x => x.TypeId).HasColumnName(nameof(Bill.TypeId));
            builder.Property(x => x.CurrencyId).HasColumnName(nameof(Bill.CurrencyId));
            builder.Property(x => x.BillId).HasColumnName(nameof(Bill.BillId));
            builder.Property(x => x.ReceiptId).HasColumnName(nameof(Bill.ReceiptId));
            builder.Property(x => x.PersonId).HasColumnName(nameof(Bill.PersonId));

            builder.Property(x => x.Description).HasColumnName(nameof(Bill.Description));
            builder.Property(x => x.DueDate).HasColumnName(nameof(Bill.DueDate));
            builder.Property(x => x.PaymentDate).HasColumnName(nameof(Bill.PaymentDate));
            builder.Property(x => x.LimitDate).HasColumnName(nameof(Bill.LimitDate));
            builder.Property(x => x.Value).HasColumnName(nameof(Bill.Value));
            builder.Property(x => x.InterestValue).HasColumnName(nameof(Bill.InterestValue));
            builder.Property(x => x.InterestRate).HasColumnName(nameof(Bill.InterestRate));
            builder.Property(x => x.InstallmentCount).HasColumnName(nameof(Bill.InstallmentCount));
            builder.Property(x => x.IsPaid).HasColumnName(nameof(Bill.IsPaid));

            builder.Property(x => x.CreatedBy).HasColumnName(nameof(Bill.CreatedBy));
            builder.Property(x => x.CreatedAt).HasColumnName(nameof(Bill.CreatedAt));
            builder.Property(x => x.UpdatedBy).HasColumnName(nameof(Bill.UpdatedBy));
            builder.Property(x => x.UpdatedAt).HasColumnName(nameof(Bill.UpdatedAt));

            builder.HasOne(x => x.File)
                .WithOne(x => x.Bill)
                .HasForeignKey<Bill>(x => x.BillId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(x => x.Receipt)
                .WithOne(x => x.Receipt)
                .HasForeignKey<Bill>(x => x.ReceiptId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(x => x.Person)
                .WithMany(x => x.Bills)
                .HasForeignKey(x => x.PersonId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Installment)
                .WithOne(x => x.Bill)
                .HasForeignKey<Bill>(x => x.InstallmentId)
                .OnDelete(DeleteBehavior.SetNull)
                .IsRequired(false);
        }
    }
}
