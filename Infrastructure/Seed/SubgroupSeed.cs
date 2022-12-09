using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Seed
{
    public static class SubgroupSeed
    {
        public static void SeedSubgroups(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Subgroup>().HasData(
                new[] {
                    new Subgroup { Id = 1, GroupId = 1, Name = "Física", Description = null, IsActivated = true, CreatedBy = 1, CreatedAt = DateTime.Now, UpdatedBy = 1, UpdatedAt = DateTime.Now },
                    new Subgroup { Id = 2, GroupId = 1, Name = "Jurídica", Description = null, IsActivated = true, CreatedBy = 1, CreatedAt = DateTime.Now, UpdatedBy = 1, UpdatedAt = DateTime.Now },
                    new Subgroup { Id = 3, GroupId = 2, Name = "Residencial", Description = null, IsActivated = true, CreatedBy = 1, CreatedAt = DateTime.Now, UpdatedBy = 1, UpdatedAt = DateTime.Now },
                    new Subgroup { Id = 4, GroupId = 2, Name = "Comercial", Description = null, IsActivated = true, CreatedBy = 1, CreatedAt = DateTime.Now, UpdatedBy = 1, UpdatedAt = DateTime.Now },
                    new Subgroup { Id = 5, GroupId = 2, Name = "Outro", Description = null, IsActivated = true, CreatedBy = 1, CreatedAt = DateTime.Now, UpdatedBy = 1, UpdatedAt = DateTime.Now },
                    new Subgroup { Id = 6, GroupId = 3, Name = "Pessoal", Description = null, IsActivated = true, CreatedBy = 1, CreatedAt = DateTime.Now, UpdatedBy = 1, UpdatedAt = DateTime.Now },
                    new Subgroup { Id = 7, GroupId = 3, Name = "Empresarial", Description = null, IsActivated = true, CreatedBy = 1, CreatedAt = DateTime.Now, UpdatedBy = 1, UpdatedAt = DateTime.Now },
                    new Subgroup { Id = 8, GroupId = 3, Name = "Outro", Description = null, IsActivated = true, CreatedBy = 1, CreatedAt = DateTime.Now, UpdatedBy = 1, UpdatedAt = DateTime.Now },
                    new Subgroup { Id = 9, GroupId = 4, Name = "Residencial", Description = null, IsActivated = true, CreatedBy = 1, CreatedAt = DateTime.Now, UpdatedBy = 1, UpdatedAt = DateTime.Now },
                    new Subgroup { Id = 10, GroupId = 4, Name = "Comercial", Description = null, IsActivated = true, CreatedBy = 1, CreatedAt = DateTime.Now, UpdatedBy = 1, UpdatedAt = DateTime.Now },
                    new Subgroup { Id = 11, GroupId = 4, Name = "Celular", Description = null, IsActivated = true, CreatedBy = 1, CreatedAt = DateTime.Now, UpdatedBy = 1, UpdatedAt = DateTime.Now },
                    new Subgroup { Id = 12, GroupId = 4, Name = "Outro", Description = null, IsActivated = true, CreatedBy = 1, CreatedAt = DateTime.Now, UpdatedBy = 1, UpdatedAt = DateTime.Now },
                    new Subgroup { Id = 13, GroupId = 5, Name = "Masculino", Description = null, IsActivated = true, CreatedBy = 1, CreatedAt = DateTime.Now, UpdatedBy = 1, UpdatedAt = DateTime.Now },
                    new Subgroup { Id = 14, GroupId = 5, Name = "Feminino", Description = null, IsActivated = true, CreatedBy = 1, CreatedAt = DateTime.Now, UpdatedBy = 1, UpdatedAt = DateTime.Now },
                    new Subgroup { Id = 15, GroupId = 5, Name = "Não Informado", Description = null, IsActivated = true, CreatedBy = 1, CreatedAt = DateTime.Now, UpdatedBy = 1, UpdatedAt = DateTime.Now },
                    new Subgroup { Id = 16, GroupId = 5, Name = "Outro", Description = null, IsActivated = true, CreatedBy = 1, CreatedAt = DateTime.Now, UpdatedBy = 1, UpdatedAt = DateTime.Now }
                }
            );
        }
    }
}
