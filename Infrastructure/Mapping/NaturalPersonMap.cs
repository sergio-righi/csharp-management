using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Infrastructure.Mapping
{
    public class NaturalPersonMap : IEntityTypeConfiguration<NaturalPerson>
    {
        public void Configure(EntityTypeBuilder<NaturalPerson> builder)
        {
            builder.ToTable(nameof(NaturalPerson));

            builder.HasKey(x => x.Id).HasAnnotation("DatabaseGenerated", DatabaseGeneratedOption.None);

            builder.Property(x => x.GenderId).HasColumnName(nameof(NaturalPerson.GenderId));

            builder.Property(x => x.GivenName).HasColumnName(nameof(NaturalPerson.GivenName));
            builder.Property(x => x.FullName).HasColumnName(nameof(NaturalPerson.FullName));
            builder.Property(x => x.SocialName).HasColumnName(nameof(NaturalPerson.SocialName));
            builder.Property(x => x.Birthday).HasColumnName(nameof(NaturalPerson.Birthday));

            builder.Property(x => x.CreatedBy).HasColumnName(nameof(NaturalPerson.CreatedBy));
            builder.Property(x => x.CreatedAt).HasColumnName(nameof(NaturalPerson.CreatedAt));
            builder.Property(x => x.UpdatedBy).HasColumnName(nameof(NaturalPerson.UpdatedBy));
            builder.Property(x => x.UpdatedAt).HasColumnName(nameof(NaturalPerson.UpdatedAt));

            builder.HasOne(x => x.Person)
                .WithOne(x => x.NaturalPerson)
                .OnDelete(DeleteBehavior.Restrict)
                .HasForeignKey<NaturalPerson>(x => x.Id);
        }
    }
}
