using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Mapping
{
    public class MenuMap : IEntityTypeConfiguration<Menu>
    {
        public void Configure(EntityTypeBuilder<Menu> builder)
        {
            builder.ToTable(nameof(Menu));

            builder.HasKey(x => x.Id);

            builder.Property(x => x.MenuId).HasColumnName(nameof(Menu.MenuId));

            builder.Property(x => x.Name).HasColumnName(nameof(Menu.Name));
            builder.Property(x => x.Description).HasColumnName(nameof(Menu.Description));
            builder.Property(x => x.Icon).HasColumnName(nameof(Menu.Icon));
            builder.Property(x => x.IsActivated).HasColumnName(nameof(Menu.IsActivated));

            builder.Property(x => x.CreatedBy).HasColumnName(nameof(Menu.CreatedBy));
            builder.Property(x => x.CreatedAt).HasColumnName(nameof(Menu.CreatedAt));
            builder.Property(x => x.UpdatedBy).HasColumnName(nameof(Menu.UpdatedBy));
            builder.Property(x => x.UpdatedAt).HasColumnName(nameof(Menu.UpdatedAt));

            builder.HasOne(x => x.Parent)
                .WithMany(x => x.Menus)
                .HasForeignKey(x => x.MenuId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired(false);
        }
    }
}
