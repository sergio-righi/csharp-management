using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Mapping
{
    public class AttendanceMap : IEntityTypeConfiguration<Attendance>
    {
        public void Configure(EntityTypeBuilder<Attendance> builder)
        {
            builder.ToTable(nameof(Attendance));

            builder.HasKey(x => x.Id);

            builder.Property(x => x.PersonId).HasColumnName(nameof(Attendance.PersonId));
            builder.Property(x => x.FileId).HasColumnName(nameof(Attendance.FileId));
            
            builder.Property(x => x.Date).HasColumnName(nameof(Attendance.Date));
            builder.Property(x => x.Observation).HasColumnName(nameof(Attendance.Observation));

            builder.Property(x => x.CreatedBy).HasColumnName(nameof(Attendance.CreatedBy));
            builder.Property(x => x.CreatedAt).HasColumnName(nameof(Attendance.CreatedAt));
            builder.Property(x => x.UpdatedBy).HasColumnName(nameof(Attendance.UpdatedBy));
            builder.Property(x => x.UpdatedAt).HasColumnName(nameof(Attendance.UpdatedAt));

            builder.HasOne(x => x.Person)
                .WithMany(x => x.Attendances)
                .HasForeignKey(x => x.PersonId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.File)
                .WithOne(x => x.Attendance)
                .HasForeignKey<Attendance>(x => x.FileId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired(false);
        }
    }
}
