using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Mapping
{
    public class PersonDocumentMap : IEntityTypeConfiguration<PersonDocument>
    {
        public void Configure(EntityTypeBuilder<PersonDocument> builder)
        {
            builder.ToTable(nameof(PersonDocument));

            builder.HasKey(x => x.Id);

            builder.Property(x => x.DocumentId).HasColumnName(nameof(PersonDocument.DocumentId));
            builder.Property(x => x.PersonId).HasColumnName(nameof(PersonDocument.PersonId));
            builder.Property(x => x.CountryId).HasColumnName(nameof(PersonDocument.CountryId));
            builder.Property(x => x.StateId).HasColumnName(nameof(PersonDocument.StateId));

            builder.Property(x => x.Number).HasColumnName(nameof(PersonDocument.Number));
            builder.Property(x => x.IssueDate).HasColumnName(nameof(PersonDocument.IssueDate));
            builder.Property(x => x.ExpiryDate).HasColumnName(nameof(PersonDocument.ExpiryDate));

            builder.Property(x => x.CreatedBy).HasColumnName(nameof(PersonDocument.CreatedBy));
            builder.Property(x => x.CreatedAt).HasColumnName(nameof(PersonDocument.CreatedAt));
            builder.Property(x => x.UpdatedBy).HasColumnName(nameof(PersonDocument.UpdatedBy));
            builder.Property(x => x.UpdatedAt).HasColumnName(nameof(PersonDocument.UpdatedAt));

            builder.HasOne(x => x.Person)
                .WithMany(x => x.Documents)
                .HasForeignKey(x => x.PersonId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Country)
                .WithMany(x => x.Documents)
                .HasForeignKey(x => x.CountryId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired(false);

            builder.HasOne(x => x.State)
                .WithMany(x => x.Documents)
                .HasForeignKey(x => x.StateId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired(false);
        }
    }
}
