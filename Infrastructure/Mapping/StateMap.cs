using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Mapping
{
    public class StateMap : IEntityTypeConfiguration<State>
    {
        public void Configure(EntityTypeBuilder<State> builder)
        {
            builder.ToTable(nameof(State));

            builder.HasKey(x => x.Id);

            builder.Property(x => x.CountryId).HasColumnName(nameof(State.CountryId));

            builder.Property(x => x.Name).HasColumnName(nameof(State.Name));
            builder.Property(x => x.PhoneCode).HasColumnName(nameof(State.PhoneCode));
            builder.Property(x => x.Abbreviation).HasColumnName(nameof(State.Abbreviation));
            builder.Property(x => x.IsActivated).HasColumnName(nameof(State.IsActivated));

            builder.Property(x => x.CreatedBy).HasColumnName(nameof(State.CreatedBy));
            builder.Property(x => x.CreatedAt).HasColumnName(nameof(State.CreatedAt));
            builder.Property(x => x.UpdatedBy).HasColumnName(nameof(State.UpdatedBy));
            builder.Property(x => x.UpdatedAt).HasColumnName(nameof(State.UpdatedAt));

            builder.HasOne(x => x.Country)
                .WithMany(x => x.States)
                .HasForeignKey(x => x.CountryId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
