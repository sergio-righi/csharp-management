using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Mapping
{
    public class FileFolderMap : IEntityTypeConfiguration<FileFolder>
    {
        public void Configure(EntityTypeBuilder<FileFolder> builder)
        {
            builder.ToTable(nameof(FileFolder));

            builder.HasKey(x => x.Id);

            builder.Property(x => x.FileId).HasColumnName(nameof(FileFolder.FileId));
            builder.Property(x => x.FolderId).HasColumnName(nameof(FileFolder.FolderId));

            builder.Property(x => x.CreatedBy).HasColumnName(nameof(FileFolder.CreatedBy));
            builder.Property(x => x.CreatedAt).HasColumnName(nameof(FileFolder.CreatedAt));
            builder.Property(x => x.UpdatedBy).HasColumnName(nameof(FileFolder.UpdatedBy));
            builder.Property(x => x.UpdatedAt).HasColumnName(nameof(FileFolder.UpdatedAt));

            builder.HasOne(x => x.File)
                .WithMany(x => x.Folders)
                .HasForeignKey(x => x.FileId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Folder)
                .WithMany(x => x.Files)
                .HasForeignKey(x => x.FolderId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
