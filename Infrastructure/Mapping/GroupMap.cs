using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Mapping
{
    public class GroupMap : IEntityTypeConfiguration<Group>
    {
        public void Configure(EntityTypeBuilder<Group> builder)
        {
            builder.ToTable(nameof(Group));

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name).HasColumnName(nameof(Group.Name));
            builder.Property(x => x.Description).HasColumnName(nameof(Group.Description));
            builder.Property(x => x.IsActivated).HasColumnName(nameof(Group.IsActivated));

            builder.Property(x => x.CreatedBy).HasColumnName(nameof(Group.CreatedBy));
            builder.Property(x => x.CreatedAt).HasColumnName(nameof(Group.CreatedAt));
            builder.Property(x => x.UpdatedBy).HasColumnName(nameof(Group.UpdatedBy));
            builder.Property(x => x.UpdatedAt).HasColumnName(nameof(Group.UpdatedAt));
        }
    }
}
