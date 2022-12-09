using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Mapping
{
    public class EmailMap : IEntityTypeConfiguration<Email>
    {
        public void Configure(EntityTypeBuilder<Email> builder)
        {
            builder.ToTable(nameof(Email));

            builder.HasKey(x => x.Id);

            builder.Property(x => x.TypeId).HasColumnName(nameof(Email.TypeId));
            builder.Property(x => x.PersonId).HasColumnName(nameof(Email.PersonId));

            builder.Property(x => x.Address).HasColumnName(nameof(Email.Address));
            builder.Property(x => x.ConfirmationDate).HasColumnName(nameof(Email.ConfirmationDate));
            builder.Property(x => x.ConfirmationToken).HasColumnName(nameof(Email.ConfirmationToken));
            builder.Property(x => x.IsDeleted).HasColumnName(nameof(Email.IsDeleted));

            builder.Property(x => x.CreatedBy).HasColumnName(nameof(Email.CreatedBy));
            builder.Property(x => x.CreatedAt).HasColumnName(nameof(Email.CreatedAt));
            builder.Property(x => x.UpdatedBy).HasColumnName(nameof(Email.UpdatedBy));
            builder.Property(x => x.UpdatedAt).HasColumnName(nameof(Email.UpdatedAt));

            builder.HasOne(x => x.Person)
                .WithMany(x => x.Emails)
                .HasForeignKey(x => x.PersonId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
