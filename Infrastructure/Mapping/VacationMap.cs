using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Mapping
{
    public class VacationMap : IEntityTypeConfiguration<Vacation>
    {
        public void Configure(EntityTypeBuilder<Vacation> builder)
        {
            builder.ToTable(nameof(Vacation));

            builder.HasKey(x => x.Id);

            builder.Property(x => x.FileId).HasColumnName(nameof(Vacation.FileId));
            builder.Property(x => x.PersonId).HasColumnName(nameof(Vacation.PersonId));

            builder.Property(x => x.StartDate).HasColumnName(nameof(Vacation.StartDate));
            builder.Property(x => x.EndDate).HasColumnName(nameof(Vacation.EndDate));
            builder.Property(x => x.Amount).HasColumnName(nameof(Vacation.Amount));
            builder.Property(x => x.ConfirmationDate).HasColumnName(nameof(Vacation.ConfirmationDate));
            builder.Property(x => x.IsConfirmed).HasColumnName(nameof(Vacation.IsConfirmed));

            builder.Property(x => x.CreatedBy).HasColumnName(nameof(Vacation.CreatedBy));
            builder.Property(x => x.CreatedAt).HasColumnName(nameof(Vacation.CreatedAt));
            builder.Property(x => x.UpdatedBy).HasColumnName(nameof(Vacation.UpdatedBy));
            builder.Property(x => x.UpdatedAt).HasColumnName(nameof(Vacation.UpdatedAt));

            builder.HasOne(x => x.Person)
                .WithMany(x => x.Vacations)
                .HasForeignKey(x => x.PersonId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.File)
                .WithMany(x => x.Vacations)
                .HasForeignKey(x => x.FileId)
                .OnDelete(DeleteBehavior.SetNull)
                .IsRequired(false);
        }
    }
}
