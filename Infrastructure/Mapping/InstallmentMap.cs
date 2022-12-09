using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Mapping
{
    public class InstallmentMap : IEntityTypeConfiguration<Installment>
    {
        public void Configure(EntityTypeBuilder<Installment> builder)
        {
            builder.ToTable(nameof(Installment));

            builder.HasKey(x => x.Id);

            builder.Property(x => x.CreatedBy).HasColumnName(nameof(Installment.CreatedBy));
            builder.Property(x => x.CreatedAt).HasColumnName(nameof(Installment.CreatedAt));
            builder.Property(x => x.UpdatedBy).HasColumnName(nameof(Installment.UpdatedBy));
            builder.Property(x => x.UpdatedAt).HasColumnName(nameof(Installment.UpdatedAt));
        }
    }
}
