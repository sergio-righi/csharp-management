using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Mapping
{
    public class InstallmentInfoMap : IEntityTypeConfiguration<InstallmentInfo>
    {
        public void Configure(EntityTypeBuilder<InstallmentInfo> builder)
        {
            builder.ToTable(nameof(InstallmentInfo));

            builder.HasKey(x => x.Id);

            builder.Property(x => x.InstallmentId).HasColumnName(nameof(InstallmentInfo.InstallmentId));

            builder.Property(x => x.Number).HasColumnName(nameof(InstallmentInfo.Number));
            builder.Property(x => x.Date).HasColumnName(nameof(InstallmentInfo.Date));
            builder.Property(x => x.ConfirmationDate).HasColumnName(nameof(InstallmentInfo.ConfirmationDate));
            builder.Property(x => x.IsConfirmed).HasColumnName(nameof(InstallmentInfo.IsConfirmed));

            builder.Property(x => x.CreatedBy).HasColumnName(nameof(InstallmentInfo.CreatedBy));
            builder.Property(x => x.CreatedAt).HasColumnName(nameof(InstallmentInfo.CreatedAt));
            builder.Property(x => x.UpdatedBy).HasColumnName(nameof(InstallmentInfo.UpdatedBy));
            builder.Property(x => x.UpdatedAt).HasColumnName(nameof(InstallmentInfo.UpdatedAt));

            builder.HasOne(x => x.Installment)
                .WithMany(x => x.Dates)
                .HasForeignKey(x => x.InstallmentId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
