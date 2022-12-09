using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Mapping
{
    public class PayrollMap : IEntityTypeConfiguration<Payroll>
    {
        public void Configure(EntityTypeBuilder<Payroll> builder)
        {
            builder.ToTable(nameof(Payroll));

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Year).HasColumnName(nameof(Payroll.Year));
            builder.Property(x => x.Month).HasColumnName(nameof(Payroll.Month));

            builder.Property(x => x.CreatedBy).HasColumnName(nameof(Payroll.CreatedBy));
            builder.Property(x => x.CreatedAt).HasColumnName(nameof(Payroll.CreatedAt));
            builder.Property(x => x.UpdatedBy).HasColumnName(nameof(Payroll.UpdatedBy));
            builder.Property(x => x.UpdatedAt).HasColumnName(nameof(Payroll.UpdatedAt));
        }
    }
}
