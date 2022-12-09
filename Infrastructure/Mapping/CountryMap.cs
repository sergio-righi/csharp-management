using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Mapping
{
    public class CountryMap : IEntityTypeConfiguration<Country>
    {
        public void Configure(EntityTypeBuilder<Country> builder)
        {
            builder.ToTable(nameof(Country));

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name).HasColumnName(nameof(Country.Name));
            builder.Property(x => x.PhoneCode).HasColumnName(nameof(Country.PhoneCode));
            builder.Property(x => x.Code).HasColumnName(nameof(Country.Code));
            builder.Property(x => x.Abbreviation).HasColumnName(nameof(Country.Abbreviation));
            builder.Property(x => x.IsActivated).HasColumnName(nameof(Country.IsActivated));

            builder.Property(x => x.CreatedBy).HasColumnName(nameof(Country.CreatedBy));
            builder.Property(x => x.CreatedAt).HasColumnName(nameof(Country.CreatedAt));
            builder.Property(x => x.UpdatedBy).HasColumnName(nameof(Country.UpdatedBy));
            builder.Property(x => x.UpdatedAt).HasColumnName(nameof(Country.UpdatedAt));
        }
    }
}
