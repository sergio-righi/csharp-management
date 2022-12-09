using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Infrastructure.Mapping
{
    public class JuridicalPersonMap : IEntityTypeConfiguration<JuridicalPerson>
    {
        public void Configure(EntityTypeBuilder<JuridicalPerson> builder)
        {
            builder.ToTable(nameof(JuridicalPerson));

            builder.HasKey(x => x.Id).HasAnnotation("DatabaseGenerated", DatabaseGeneratedOption.None);

            builder.Property(x => x.ActivityId).HasColumnName(nameof(JuridicalPerson.ActivityId));
            builder.Property(x => x.TypeId).HasColumnName(nameof(JuridicalPerson.TypeId));
            builder.Property(x => x.AreaId).HasColumnName(nameof(JuridicalPerson.AreaId));

            builder.Property(x => x.FantasyName).HasColumnName(nameof(JuridicalPerson.FantasyName));
            builder.Property(x => x.CompanyName).HasColumnName(nameof(JuridicalPerson.CompanyName));
            builder.Property(x => x.Description).HasColumnName(nameof(JuridicalPerson.Description));
            builder.Property(x => x.CNPJ).HasColumnName(nameof(JuridicalPerson.CNPJ));

            builder.Property(x => x.CreatedBy).HasColumnName(nameof(JuridicalPerson.CreatedBy));
            builder.Property(x => x.CreatedAt).HasColumnName(nameof(JuridicalPerson.CreatedAt));
            builder.Property(x => x.UpdatedBy).HasColumnName(nameof(JuridicalPerson.UpdatedBy));
            builder.Property(x => x.UpdatedAt).HasColumnName(nameof(JuridicalPerson.UpdatedAt));

            builder.HasOne(x => x.Person)
                .WithOne(x => x.JuridicalPerson)
                .OnDelete(DeleteBehavior.Restrict)
                .HasForeignKey<JuridicalPerson>(x => x.Id);
        }
    }
}
