using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Mapping
{
    public class PhoneMap : IEntityTypeConfiguration<Phone>
    {
        public void Configure(EntityTypeBuilder<Phone> builder)
        {
            builder.ToTable(nameof(Phone));

            builder.HasKey(x => x.Id);

            builder.Property(x => x.TypeId).HasColumnName(nameof(Phone.TypeId));
            builder.Property(x => x.PersonId).HasColumnName(nameof(Phone.PersonId));

            builder.Property(x => x.Number).HasColumnName(nameof(Phone.Number));
            builder.Property(x => x.IsDeleted).HasColumnName(nameof(Phone.IsDeleted));

            builder.Property(x => x.CreatedBy).HasColumnName(nameof(Phone.CreatedBy));
            builder.Property(x => x.CreatedAt).HasColumnName(nameof(Phone.CreatedAt));
            builder.Property(x => x.UpdatedBy).HasColumnName(nameof(Phone.UpdatedBy));
            builder.Property(x => x.UpdatedAt).HasColumnName(nameof(Phone.UpdatedAt));

            builder.HasOne(x => x.Person)
                .WithMany(x => x.Phones)
                .HasForeignKey(x => x.PersonId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
