using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Mapping
{
    public class PersonTokenMap : IEntityTypeConfiguration<PersonToken>
    {
        public void Configure(EntityTypeBuilder<PersonToken> builder)
        {
            builder.ToTable(nameof(PersonToken));

            builder.HasKey(x => x.Id);

            builder.Property(x => x.PersonId).HasColumnName(nameof(PersonToken.PersonId));
            builder.Property(x => x.TokenId).HasColumnName(nameof(PersonToken.TokenId));

            builder.Property(x => x.ConfirmationDate).HasColumnName(nameof(PersonToken.ConfirmationDate));
            builder.Property(x => x.Token).HasColumnName(nameof(PersonToken.Token));
            builder.Property(x => x.IpAddress).HasColumnName(nameof(PersonToken.IpAddress));
            builder.Property(x => x.ExpiryDate).HasColumnName(nameof(PersonToken.ExpiryDate));

            builder.Property(x => x.CreatedBy).HasColumnName(nameof(PersonToken.CreatedBy));
            builder.Property(x => x.CreatedAt).HasColumnName(nameof(PersonToken.CreatedAt));
            builder.Property(x => x.UpdatedBy).HasColumnName(nameof(PersonToken.UpdatedBy));
            builder.Property(x => x.UpdatedAt).HasColumnName(nameof(PersonToken.UpdatedAt));

            builder.HasOne(x => x.Person)
                .WithMany(x => x.Tokes)
                .HasForeignKey(x => x.PersonId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
