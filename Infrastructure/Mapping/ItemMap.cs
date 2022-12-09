using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Mapping
{
    public class ItemMap : IEntityTypeConfiguration<Item>
    {
        public void Configure(EntityTypeBuilder<Item> builder)
        {
            builder.ToTable(nameof(Item));

            builder.HasKey(x => x.Id);

            builder.Property(x => x.ItemId).HasColumnName(nameof(Item.ItemId));

            builder.Property(x => x.Name).HasColumnName(nameof(Item.Name));
            builder.Property(x => x.Description).HasColumnName(nameof(Item.Description));
            builder.Property(x => x.IsActivated).HasColumnName(nameof(Item.IsActivated));
            builder.Property(x => x.IsDeleted).HasColumnName(nameof(Item.IsDeleted));

            builder.Property(x => x.CreatedBy).HasColumnName(nameof(Item.CreatedBy));
            builder.Property(x => x.CreatedAt).HasColumnName(nameof(Item.CreatedAt));
            builder.Property(x => x.UpdatedBy).HasColumnName(nameof(Item.UpdatedBy));
            builder.Property(x => x.UpdatedAt).HasColumnName(nameof(Item.UpdatedAt));

            builder.HasOne(x => x.Parent)
                .WithMany(x => x.Items)
                .HasForeignKey(x => x.ItemId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired(false);
        }
    }
}
