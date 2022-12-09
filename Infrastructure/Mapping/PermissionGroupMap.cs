using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Mapping
{
    public class PermissionGroupMap : IEntityTypeConfiguration<PermissionGroup>
    {
        public void Configure(EntityTypeBuilder<PermissionGroup> builder)
        {
            builder.ToTable(nameof(PermissionGroup));

            builder.HasKey(x => x.Id);

            builder.Property(x => x.RoleId).HasColumnName(nameof(PermissionGroup.RoleId));
            builder.Property(x => x.PersonId).HasColumnName(nameof(PermissionGroup.PersonId));
            builder.Property(x => x.PermissionId).HasColumnName(nameof(PermissionGroup.PermissionId));
            
            builder.Property(x => x.Add).HasColumnName(nameof(PermissionGroup.Add));
            builder.Property(x => x.Update).HasColumnName(nameof(PermissionGroup.Update));
            builder.Property(x => x.Print).HasColumnName(nameof(PermissionGroup.Print));
            builder.Property(x => x.Read).HasColumnName(nameof(PermissionGroup.Read));
            builder.Property(x => x.IsDeleted).HasColumnName(nameof(PermissionGroup.IsDeleted));

            builder.Property(x => x.CreatedBy).HasColumnName(nameof(PermissionGroup.CreatedBy));
            builder.Property(x => x.CreatedAt).HasColumnName(nameof(PermissionGroup.CreatedAt));
            builder.Property(x => x.UpdatedBy).HasColumnName(nameof(PermissionGroup.UpdatedBy));
            builder.Property(x => x.UpdatedAt).HasColumnName(nameof(PermissionGroup.UpdatedAt));

            builder.HasOne(x => x.Permission)
                .WithMany(x => x.Permissions)
                .HasForeignKey(x => x.PermissionId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Person)
                .WithMany(x => x.PermissionGroups)
                .HasForeignKey(x => x.PersonId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
