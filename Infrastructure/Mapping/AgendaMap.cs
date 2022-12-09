using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Mapping
{
    public class AgendaMap : IEntityTypeConfiguration<Agenda>
    {
        public void Configure(EntityTypeBuilder<Agenda> builder)
        {
            builder.ToTable(nameof(Agenda));

            builder.HasKey(x => x.Id);

            builder.Property(x => x.CustomerId).HasColumnName(nameof(Agenda.CustomerId));
            builder.Property(x => x.AddressId).HasColumnName(nameof(Agenda.AddressId));
            
            builder.Property(x => x.Date).HasColumnName(nameof(Agenda.Date));
            builder.Property(x => x.StartTime).HasColumnName(nameof(Agenda.StartTime));
            builder.Property(x => x.EndTime).HasColumnName(nameof(Agenda.EndTime));
            builder.Property(x => x.Description).HasColumnName(nameof(Agenda.Description));

            builder.Property(x => x.CreatedBy).HasColumnName(nameof(Agenda.CreatedBy));
            builder.Property(x => x.CreatedAt).HasColumnName(nameof(Agenda.CreatedAt));
            builder.Property(x => x.UpdatedBy).HasColumnName(nameof(Agenda.UpdatedBy));
            builder.Property(x => x.UpdatedAt).HasColumnName(nameof(Agenda.UpdatedAt));

            builder.HasOne(x => x.Customer)
                .WithMany(x => x.Agenda)
                .HasForeignKey(x => x.CustomerId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired(false);

            builder.HasOne(x => x.Address)
                .WithMany(x => x.Agenda)
                .HasForeignKey(x => x.AddressId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired(false);
        }
    }
}
