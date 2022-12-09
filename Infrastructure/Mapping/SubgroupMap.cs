using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Mapping
{
    public class SubgroupMap : IEntityTypeConfiguration<Subgroup>
    {
        public void Configure(EntityTypeBuilder<Subgroup> builder)
        {
            builder.ToTable(nameof(Subgroup));

            builder.HasKey(x => x.Id);

            builder.Property(x => x.GroupId).HasColumnName(nameof(Subgroup.GroupId));

            builder.Property(x => x.Name).HasColumnName(nameof(Subgroup.Name));
            builder.Property(x => x.Description).HasColumnName(nameof(Subgroup.Description));
            builder.Property(x => x.IsActivated).HasColumnName(nameof(Subgroup.IsActivated));

            builder.Property(x => x.CreatedBy).HasColumnName(nameof(Subgroup.CreatedBy));
            builder.Property(x => x.CreatedAt).HasColumnName(nameof(Subgroup.CreatedAt));
            builder.Property(x => x.UpdatedBy).HasColumnName(nameof(Subgroup.UpdatedBy));
            builder.Property(x => x.UpdatedAt).HasColumnName(nameof(Subgroup.UpdatedAt));

            builder.HasOne(x => x.Group)
                .WithMany(x => x.Subgroups)
                .HasForeignKey(x => x.GroupId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
