using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Mapping
{
    public class FolderMap : IEntityTypeConfiguration<Folder>
    {
        public void Configure(EntityTypeBuilder<Folder> builder)
        {
            builder.ToTable(nameof(Folder));

            builder.HasKey(x => x.Id);

            builder.Property(x => x.FolderId).HasColumnName(nameof(Folder.FolderId));

            builder.Property(x => x.Name).HasColumnName(nameof(Folder.Name));

            builder.Property(x => x.CreatedBy).HasColumnName(nameof(Folder.CreatedBy));
            builder.Property(x => x.CreatedAt).HasColumnName(nameof(Folder.CreatedAt));
            builder.Property(x => x.UpdatedBy).HasColumnName(nameof(Folder.UpdatedBy));
            builder.Property(x => x.UpdatedAt).HasColumnName(nameof(Folder.UpdatedAt));

            builder.HasOne(x => x.Parent)
                .WithMany(x => x.Folders)
                .HasForeignKey(x => x.FolderId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired(false);
        }
    }
}
