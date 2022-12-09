using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Mapping
{
    public class PersonTimetableMap : IEntityTypeConfiguration<PersonTimetable>
    {
        public void Configure(EntityTypeBuilder<PersonTimetable> builder)
        {
            builder.ToTable(nameof(PersonTimetable));

            builder.HasKey(x => x.Id);

            builder.Property(x => x.PersonId).HasColumnName(nameof(PersonTimetable.PersonId));
            builder.Property(x => x.TimetableId).HasColumnName(nameof(PersonTimetable.TimetableId));
            
            builder.Property(x => x.CreatedBy).HasColumnName(nameof(PersonTimetable.CreatedBy));
            builder.Property(x => x.CreatedAt).HasColumnName(nameof(PersonTimetable.CreatedAt));
            builder.Property(x => x.UpdatedBy).HasColumnName(nameof(PersonTimetable.UpdatedBy));
            builder.Property(x => x.UpdatedAt).HasColumnName(nameof(PersonTimetable.UpdatedAt));

            builder.HasOne(x => x.Person)
                .WithMany(x => x.PersonTimetable)
                .HasForeignKey(x => x.PersonId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Timetable)
                .WithMany(x => x.PersonTimetable)
                .HasForeignKey(x => x.TimetableId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
