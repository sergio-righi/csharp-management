using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Mapping
{
    public class PersonRoleMap : IEntityTypeConfiguration<PersonRole>
    {
        public void Configure(EntityTypeBuilder<PersonRole> builder)
        {
            builder.ToTable(nameof(PersonRole));

            builder.HasKey(x => x.Id);

            builder.Property(x => x.PersonId).HasColumnName(nameof(PersonRole.PersonId));
            builder.Property(x => x.RoleId).HasColumnName(nameof(PersonRole.RoleId));
            builder.Property(x => x.StartDate).HasColumnName(nameof(PersonRole.StartDate));
            builder.Property(x => x.EndDate).HasColumnName(nameof(PersonRole.EndDate));
            
            builder.Property(x => x.CreatedBy).HasColumnName(nameof(PersonRole.CreatedBy));
            builder.Property(x => x.CreatedAt).HasColumnName(nameof(PersonRole.CreatedAt));
            builder.Property(x => x.UpdatedBy).HasColumnName(nameof(PersonRole.UpdatedBy));
            builder.Property(x => x.UpdatedAt).HasColumnName(nameof(PersonRole.UpdatedAt));

            builder.HasOne(x => x.Person)
                .WithMany(x => x.Roles)
                .HasForeignKey(x => x.PersonId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
