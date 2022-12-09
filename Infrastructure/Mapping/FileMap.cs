using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Mapping
{
    public class FileMap : IEntityTypeConfiguration<File>
    {
        public void Configure(EntityTypeBuilder<File> builder)
        {
            builder.ToTable(nameof(File));

            builder.HasKey(x => x.Id);

            builder.Property(x => x.ExtensionId).HasColumnName(nameof(File.ExtensionId));

            builder.Property(x => x.Name).HasColumnName(nameof(File.Name));
            builder.Property(x => x.Description).HasColumnName(nameof(File.Description));
            builder.Property(x => x.GeneratedName).HasColumnName(nameof(File.GeneratedName));
            builder.Property(x => x.Extension).HasColumnName(nameof(File.Extension));
            builder.Property(x => x.Key).HasColumnName(nameof(File.Key));
            builder.Property(x => x.Size).HasColumnName(nameof(File.Size));
            builder.Property(x => x.IsActivated).HasColumnName(nameof(File.IsActivated));
            builder.Property(x => x.IsDeleted).HasColumnName(nameof(File.IsDeleted));

            builder.Property(x => x.CreatedBy).HasColumnName(nameof(File.CreatedBy));
            builder.Property(x => x.CreatedAt).HasColumnName(nameof(File.CreatedAt));
            builder.Property(x => x.UpdatedBy).HasColumnName(nameof(File.UpdatedBy));
            builder.Property(x => x.UpdatedAt).HasColumnName(nameof(File.UpdatedAt));
        }
    }
}
