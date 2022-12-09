using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Mapping
{
    public class AddressMap : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> builder)
        {
            builder.ToTable(nameof(Address));

            builder.HasKey(x => x.Id);

            builder.Property(x => x.CityId).HasColumnName(nameof(Address.CityId));
            builder.Property(x => x.PersonId).HasColumnName(nameof(Address.PersonId));
            builder.Property(x => x.TypeId).HasColumnName(nameof(Address.TypeId));

            builder.Property(x => x.Zipcode).HasColumnName(nameof(Address.Zipcode));
            builder.Property(x => x.Street).HasColumnName(nameof(Address.Street));
            builder.Property(x => x.Number).HasColumnName(nameof(Address.Number));
            builder.Property(x => x.Complement).HasColumnName(nameof(Address.Complement));
            builder.Property(x => x.Neighborhood).HasColumnName(nameof(Address.Neighborhood));
            builder.Property(x => x.IsDeleted).HasColumnName(nameof(Address.IsDeleted));

            builder.Property(x => x.CreatedBy).HasColumnName(nameof(Address.CreatedBy));
            builder.Property(x => x.CreatedAt).HasColumnName(nameof(Address.CreatedAt));
            builder.Property(x => x.UpdatedBy).HasColumnName(nameof(Address.UpdatedBy));
            builder.Property(x => x.UpdatedAt).HasColumnName(nameof(Address.UpdatedAt));

            builder.HasOne(x => x.City)
                .WithMany(x => x.Addresses)
                .HasForeignKey(x => x.CityId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Person)
                .WithMany(x => x.Addresses)
                .HasForeignKey(x => x.PersonId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
