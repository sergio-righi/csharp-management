using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Mapping
{
    public class PaycheckMap : IEntityTypeConfiguration<Paycheck>
    {
        public void Configure(EntityTypeBuilder<Paycheck> builder)
        {
            builder.ToTable(nameof(Paycheck));

            builder.HasKey(x => x.Id);

            builder.Property(x => x.FileId).HasColumnName(nameof(Paycheck.FileId));
            builder.Property(x => x.PayrollId).HasColumnName(nameof(Paycheck.PayrollId));
            builder.Property(x => x.PersonId).HasColumnName(nameof(Paycheck.PersonId));

            builder.Property(x => x.RegularHour).HasColumnName(nameof(Paycheck.RegularHour));
            builder.Property(x => x.HourRate).HasColumnName(nameof(Paycheck.HourRate));
            builder.Property(x => x.OvertimeCount).HasColumnName(nameof(Paycheck.OvertimeCount));
            builder.Property(x => x.OvertimeValue).HasColumnName(nameof(Paycheck.OvertimeValue));
            builder.Property(x => x.GrossEarning).HasColumnName(nameof(Paycheck.GrossEarning));
            builder.Property(x => x.Bonus).HasColumnName(nameof(Paycheck.Bonus));
            builder.Property(x => x.Tax).HasColumnName(nameof(Paycheck.Tax));
            builder.Property(x => x.Deduction).HasColumnName(nameof(Paycheck.Deduction));
            builder.Property(x => x.Observation).HasColumnName(nameof(Paycheck.Observation));
            builder.Property(x => x.PaymentDate).HasColumnName(nameof(Paycheck.PaymentDate));
            builder.Property(x => x.ConfirmationDate).HasColumnName(nameof(Paycheck.ConfirmationDate));
            builder.Property(x => x.IsExtra).HasColumnName(nameof(Paycheck.IsExtra));
            builder.Property(x => x.IsConfirmed).HasColumnName(nameof(Paycheck.IsConfirmed));

            builder.Property(x => x.CreatedBy).HasColumnName(nameof(Paycheck.CreatedBy));
            builder.Property(x => x.CreatedAt).HasColumnName(nameof(Paycheck.CreatedAt));
            builder.Property(x => x.UpdatedBy).HasColumnName(nameof(Paycheck.UpdatedBy));
            builder.Property(x => x.UpdatedAt).HasColumnName(nameof(Paycheck.UpdatedAt));

            builder.HasOne(x => x.File)
                .WithMany(x => x.Paychecks)
                .HasForeignKey(x => x.FileId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired(false);

            builder.HasOne(x => x.Person)
                .WithMany(x => x.Paychecks)
                .HasForeignKey(x => x.PersonId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Payroll)
                .WithMany(x => x.Paychecks)
                .HasForeignKey(x => x.PayrollId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
