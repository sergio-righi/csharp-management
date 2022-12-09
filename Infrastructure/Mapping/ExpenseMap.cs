using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Mapping
{
    public class ExpenseMap : IEntityTypeConfiguration<Expense>
    {
        public void Configure(EntityTypeBuilder<Expense> builder)
        {
            builder.ToTable(nameof(Expense));

            builder.HasKey(x => x.Id);

            builder.Property(x => x.PersonId).HasColumnName(nameof(Expense.PersonId));
            builder.Property(x => x.ReceiptId).HasColumnName(nameof(Expense.ReceiptId));

            builder.Property(x => x.Date).HasColumnName(nameof(Expense.Date));
            builder.Property(x => x.Value).HasColumnName(nameof(Expense.Value));
            builder.Property(x => x.Description).HasColumnName(nameof(Expense.Description));

            builder.Property(x => x.CreatedBy).HasColumnName(nameof(Expense.CreatedBy));
            builder.Property(x => x.CreatedAt).HasColumnName(nameof(Expense.CreatedAt));
            builder.Property(x => x.UpdatedBy).HasColumnName(nameof(Expense.UpdatedBy));
            builder.Property(x => x.UpdatedAt).HasColumnName(nameof(Expense.UpdatedAt));

            builder.HasOne(x => x.Person)
                .WithMany(x => x.Expenses)
                .HasForeignKey(x => x.PersonId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Receipt)
                .WithMany(x => x.Expenses)
                .HasForeignKey(x => x.ReceiptId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
