using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Mapping
{
    public class PersonAgendaMap : IEntityTypeConfiguration<PersonAgenda>
    {
        public void Configure(EntityTypeBuilder<PersonAgenda> builder)
        {
            builder.ToTable(nameof(PersonAgenda));

            builder.HasKey(x => x.Id);

            builder.Property(x => x.PersonId).HasColumnName(nameof(PersonAgenda.PersonId));
            builder.Property(x => x.AgendaId).HasColumnName(nameof(PersonAgenda.AgendaId));

            builder.Property(x => x.CreatedBy).HasColumnName(nameof(PersonAgenda.CreatedBy));
            builder.Property(x => x.CreatedAt).HasColumnName(nameof(PersonAgenda.CreatedAt));
            builder.Property(x => x.UpdatedBy).HasColumnName(nameof(PersonAgenda.UpdatedBy));
            builder.Property(x => x.UpdatedAt).HasColumnName(nameof(PersonAgenda.UpdatedAt));

            builder.HasOne(x => x.Person)
                .WithMany(x => x.PersonAgenda)
                .HasForeignKey(x => x.PersonId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Agenda)
                .WithMany(x => x.PersonAgenda)
                .HasForeignKey(x => x.AgendaId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
