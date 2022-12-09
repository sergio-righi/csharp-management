using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Mapping
{
    public class PersonMap : IEntityTypeConfiguration<Person>
    {
        public void Configure(EntityTypeBuilder<Person> builder)
        {
            builder.ToTable(nameof(Person));

            builder.HasKey(x => x.Id);

            builder.Property(x => x.PhotoId).HasColumnName(nameof(Person.PhotoId));

            builder.Property(x => x.TypeId).HasColumnName(nameof(Person.TypeId));
            builder.Property(x => x.Login).HasColumnName(nameof(Person.Login));
            builder.Property(x => x.Password).HasColumnName(nameof(Person.Password));
            builder.Property(x => x.IsActivated).HasColumnName(nameof(Person.IsActivated));
            builder.Property(x => x.IsDeleted).HasColumnName(nameof(Person.IsDeleted));

            builder.Property(x => x.CreatedBy).HasColumnName(nameof(Person.CreatedBy));
            builder.Property(x => x.CreatedAt).HasColumnName(nameof(Person.CreatedAt));
            builder.Property(x => x.UpdatedBy).HasColumnName(nameof(Person.UpdatedBy));
            builder.Property(x => x.UpdatedAt).HasColumnName(nameof(Person.UpdatedAt));

            builder.HasOne(x => x.Photo)
                .WithOne(x => x.Person)
                .HasForeignKey<Person>(x => x.PhotoId)
                .OnDelete(DeleteBehavior.SetNull)
                .IsRequired(false);
        }
    }
}
