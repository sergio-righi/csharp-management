using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Mapping
{
    public class ExamMap : IEntityTypeConfiguration<Exam>
    {
        public void Configure(EntityTypeBuilder<Exam> builder)
        {
            builder.ToTable(nameof(Exam));

            builder.HasKey(x => x.Id);

            builder.Property(x => x.FileId).HasColumnName(nameof(Exam.FileId));
            builder.Property(x => x.PersonId).HasColumnName(nameof(Exam.PersonId));
            builder.Property(x => x.ExamId).HasColumnName(nameof(Exam.ExamId));

            builder.Property(x => x.Name).HasColumnName(nameof(Exam.Name));
            builder.Property(x => x.Description).HasColumnName(nameof(Exam.Description));
            builder.Property(x => x.Date).HasColumnName(nameof(Exam.Date));

            builder.Property(x => x.CreatedBy).HasColumnName(nameof(Exam.CreatedBy));
            builder.Property(x => x.CreatedAt).HasColumnName(nameof(Exam.CreatedAt));
            builder.Property(x => x.UpdatedBy).HasColumnName(nameof(Exam.UpdatedBy));
            builder.Property(x => x.UpdatedAt).HasColumnName(nameof(Exam.UpdatedAt));

            builder.HasOne(x => x.Person)
                .WithMany(x => x.Exams)
                .HasForeignKey(x => x.PersonId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.File)
                .WithMany(x => x.Exams)
                .HasForeignKey(x => x.FileId)
                .OnDelete(DeleteBehavior.SetNull)
                .IsRequired(false);
        }
    }
}
