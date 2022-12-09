using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Mapping
{
    public class OvertimeMap : IEntityTypeConfiguration<Overtime>
    {
        public void Configure(EntityTypeBuilder<Overtime> builder)
        {
            builder.ToTable(nameof(Overtime));

            builder.HasKey(x => x.Id);

            builder.Property(x => x.FileId).HasColumnName(nameof(Overtime.FileId));
            builder.Property(x => x.PersonId).HasColumnName(nameof(Overtime.PersonId));

            builder.Property(x => x.Date).HasColumnName(nameof(Overtime.Date));
            builder.Property(x => x.Count).HasColumnName(nameof(Overtime.Count));
            builder.Property(x => x.Earning).HasColumnName(nameof(Overtime.Earning));
            builder.Property(x => x.PaymentDate).HasColumnName(nameof(Overtime.PaymentDate));
            builder.Property(x => x.ConfirmationDate).HasColumnName(nameof(Overtime.ConfirmationDate));
            builder.Property(x => x.IsConfirmed).HasColumnName(nameof(Overtime.IsConfirmed));
            builder.Property(x => x.IsPaid).HasColumnName(nameof(Overtime.IsPaid));

            builder.Property(x => x.CreatedBy).HasColumnName(nameof(Overtime.CreatedBy));
            builder.Property(x => x.CreatedAt).HasColumnName(nameof(Overtime.CreatedAt));
            builder.Property(x => x.UpdatedBy).HasColumnName(nameof(Overtime.UpdatedBy));
            builder.Property(x => x.UpdatedAt).HasColumnName(nameof(Overtime.UpdatedAt));

            builder.HasOne(x => x.Person)
                .WithMany(x => x.Overtimes)
                .HasForeignKey(x => x.PersonId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.File)
                .WithMany(x => x.Overtimes)
                .HasForeignKey(x => x.FileId)
                .OnDelete(DeleteBehavior.SetNull)
                .IsRequired(false);
        }
    }
}
