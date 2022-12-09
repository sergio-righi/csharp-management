using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Mapping
{
    public class TimetableMap : IEntityTypeConfiguration<Timetable>
    {
        public void Configure(EntityTypeBuilder<Timetable> builder)
        {
            builder.ToTable(nameof(Timetable));

            builder.HasKey(x => x.Id);

            builder.Property(x => x.ShiftId).HasColumnName(nameof(Timetable.ShiftId));
            
            builder.Property(x => x.Weekday).HasColumnName(nameof(Timetable.Weekday));
            builder.Property(x => x.StartTime).HasColumnName(nameof(Timetable.StartTime));
            builder.Property(x => x.EndTime).HasColumnName(nameof(Timetable.EndTime));
            builder.Property(x => x.IsActivated).HasColumnName(nameof(Timetable.IsActivated));

            builder.Property(x => x.CreatedBy).HasColumnName(nameof(Timetable.CreatedBy));
            builder.Property(x => x.CreatedAt).HasColumnName(nameof(Timetable.CreatedAt));
            builder.Property(x => x.UpdatedBy).HasColumnName(nameof(Timetable.UpdatedBy));
            builder.Property(x => x.UpdatedAt).HasColumnName(nameof(Timetable.UpdatedAt));
        }
    }
}
