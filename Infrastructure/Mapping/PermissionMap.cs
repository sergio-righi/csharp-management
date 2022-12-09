using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Mapping
{
    public class PermissionMap : IEntityTypeConfiguration<Permission>
    {
        public void Configure(EntityTypeBuilder<Permission> builder)
        {
            builder.ToTable(nameof(Permission));

            builder.HasKey(x => x.Id);

            builder.Property(x => x.MenuId).HasColumnName(nameof(Permission.MenuId));

            builder.Property(x => x.CreatedBy).HasColumnName(nameof(Permission.CreatedBy));
            builder.Property(x => x.CreatedAt).HasColumnName(nameof(Permission.CreatedAt));
            builder.Property(x => x.UpdatedBy).HasColumnName(nameof(Permission.UpdatedBy));
            builder.Property(x => x.UpdatedAt).HasColumnName(nameof(Permission.UpdatedAt));

            builder.HasOne(x => x.Menu)
                .WithMany(x => x.Permissions)
                .HasForeignKey(x => x.MenuId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
