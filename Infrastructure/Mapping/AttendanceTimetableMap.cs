using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Mapping
{
    public class AttendanceTimetableMap : IEntityTypeConfiguration<AttendanceTimetable>
    {
        public void Configure(EntityTypeBuilder<AttendanceTimetable> builder)
        {
            builder.ToTable(nameof(AttendanceTimetable));

            builder.HasKey(x => x.Id);

            builder.Property(x => x.AttendanceId).HasColumnName(nameof(AttendanceTimetable.AttendanceId));
            builder.Property(x => x.TimetableId).HasColumnName(nameof(AttendanceTimetable.TimetableId));
            builder.Property(x => x.SituationId).HasColumnName(nameof(AttendanceTimetable.SituationId));
            
            builder.Property(x => x.Percentage).HasColumnName(nameof(AttendanceTimetable.Percentage));

            builder.Property(x => x.CreatedBy).HasColumnName(nameof(AttendanceTimetable.CreatedBy));
            builder.Property(x => x.CreatedAt).HasColumnName(nameof(AttendanceTimetable.CreatedAt));
            builder.Property(x => x.UpdatedBy).HasColumnName(nameof(AttendanceTimetable.UpdatedBy));
            builder.Property(x => x.UpdatedAt).HasColumnName(nameof(AttendanceTimetable.UpdatedAt));

            builder.HasOne(x => x.Attendance)
                .WithMany(x => x.AttendanceTimetable)
                .HasForeignKey(x => x.AttendanceId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Timetable)
                .WithMany(x => x.AttendanceTimetable)
                .HasForeignKey(x => x.TimetableId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
