using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Seed
{
    public static class GroupSeed
    {
        public static void SeedGroups(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Group>().HasData(
                new[] {
                    new Group { Id = 1, Name = "Tipo de Pessoa", Description = null, IsActivated = true, CreatedBy = 1, CreatedAt = DateTime.Now, UpdatedBy = 1, UpdatedAt = DateTime.Now },
                    new Group { Id = 2, Name = "Tipo de Endereço", Description = null, IsActivated = true, CreatedBy = 1, CreatedAt = DateTime.Now, UpdatedBy = 1, UpdatedAt = DateTime.Now },
                    new Group { Id = 3, Name = "Tipo de Email", Description = null, IsActivated = true, CreatedBy = 1, CreatedAt = DateTime.Now, UpdatedBy = 1, UpdatedAt = DateTime.Now },
                    new Group { Id = 4, Name = "Tipo de Telefone", Description = null, IsActivated = true, CreatedBy = 1, CreatedAt = DateTime.Now, UpdatedBy = 1, UpdatedAt = DateTime.Now },
                    new Group { Id = 5, Name = "Tipo de Gênero", Description = null, IsActivated = true, CreatedBy = 1, CreatedAt = DateTime.Now, UpdatedBy = 1, UpdatedAt = DateTime.Now }
                }
            );

        }
    }
}
