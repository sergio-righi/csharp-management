using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Mapping
{
    public class CityMap : IEntityTypeConfiguration<City>
    {
        public void Configure(EntityTypeBuilder<City> builder)
        {
            builder.ToTable(nameof(City));

            builder.HasKey(x => x.Id);

            builder.Property(x => x.StateId).HasColumnName(nameof(City.StateId));
            builder.Property(x => x.CountryId).HasColumnName(nameof(City.CountryId));

            builder.Property(x => x.Name).HasColumnName(nameof(City.Name));
            builder.Property(x => x.PhoneCode).HasColumnName(nameof(City.PhoneCode));
            builder.Property(x => x.IsActivated).HasColumnName(nameof(City.IsActivated));

            builder.Property(x => x.CreatedBy).HasColumnName(nameof(City.CreatedBy));
            builder.Property(x => x.CreatedAt).HasColumnName(nameof(City.CreatedAt));
            builder.Property(x => x.UpdatedBy).HasColumnName(nameof(City.UpdatedBy));
            builder.Property(x => x.UpdatedAt).HasColumnName(nameof(City.UpdatedAt));

            builder.HasOne(x => x.State)
                .WithMany(x => x.Citites)
                .HasForeignKey(x => x.StateId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Country)
                .WithMany(x => x.Citites)
                .HasForeignKey(x => x.CountryId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
