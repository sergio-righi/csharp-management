using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Seed
{
    public static class ItemSeed
    {
        public static void SeedItems(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Item>().HasData(
                new[] {
                    new Item { Id = 1, ItemId = null, Name = "Indication", Description = null, IsActivated = true, IsDeleted = false, CreatedBy = 1, CreatedAt = DateTime.Now, UpdatedBy = 1, UpdatedAt = DateTime.Now },
                    new Item { Id = 2, ItemId = null, Name = "Software", Description = null, IsActivated = true, IsDeleted = false, CreatedBy = 1, CreatedAt = DateTime.Now, UpdatedBy = 1, UpdatedAt = DateTime.Now },
                    new Item { Id = 3, ItemId = null, Name = "Sector", Description = null, IsActivated = true, IsDeleted = false, CreatedBy = 1, CreatedAt = DateTime.Now, UpdatedBy = 1, UpdatedAt = DateTime.Now },
                    new Item { Id = 4, ItemId = null, Name = "Story", Description = null, IsActivated = true, IsDeleted = false, CreatedBy = 1, CreatedAt = DateTime.Now, UpdatedBy = 1, UpdatedAt = DateTime.Now },
                    new Item { Id = 5, ItemId = 4, Name = "Criticism", Description = null, IsActivated = true, IsDeleted = false, CreatedBy = 1, CreatedAt = DateTime.Now, UpdatedBy = 1, UpdatedAt = DateTime.Now },
                    new Item { Id = 6, ItemId = 4, Name = "Recipe", Description = null, IsActivated = true, IsDeleted = false, CreatedBy = 1, CreatedAt = DateTime.Now, UpdatedBy = 1, UpdatedAt = DateTime.Now },
                    new Item { Id = 7, ItemId = 4, Name = "Sister", Description = null, IsActivated = true, IsDeleted = false, CreatedBy = 1, CreatedAt = DateTime.Now, UpdatedBy = 1, UpdatedAt = DateTime.Now },
                    new Item { Id = 8, ItemId = 4, Name = "Department", Description = null, IsActivated = true, IsDeleted = false, CreatedBy = 1, CreatedAt = DateTime.Now, UpdatedBy = 1, UpdatedAt = DateTime.Now },
                    new Item { Id = 9, ItemId = 11, Name = "Assumption", Description = null, IsActivated = true, IsDeleted = false, CreatedBy = 1, CreatedAt = DateTime.Now, UpdatedBy = 1, UpdatedAt = DateTime.Now },
                    new Item { Id = 10, ItemId = 11, Name = "Affair", Description = null, IsActivated = true, IsDeleted = false, CreatedBy = 1, CreatedAt = DateTime.Now, UpdatedBy = 1, UpdatedAt = DateTime.Now },
                    new Item { Id = 11, ItemId = 11, Name = "Church", Description = null, IsActivated = true, IsDeleted = false, CreatedBy = 1, CreatedAt = DateTime.Now, UpdatedBy = 1, UpdatedAt = DateTime.Now },
                    new Item { Id = 12, ItemId = 11, Name = "Memory", Description = null, IsActivated = true, IsDeleted = false, CreatedBy = 1, CreatedAt = DateTime.Now, UpdatedBy = 1, UpdatedAt = DateTime.Now },
                    new Item { Id = 13, ItemId = 5, Name = "Permission", Description = null, IsActivated = true, IsDeleted = false, CreatedBy = 1, CreatedAt = DateTime.Now, UpdatedBy = 1, UpdatedAt = DateTime.Now },
                    new Item { Id = 14, ItemId = 5, Name = "King", Description = null, IsActivated = true, IsDeleted = false, CreatedBy = 1, CreatedAt = DateTime.Now, UpdatedBy = 1, UpdatedAt = DateTime.Now },
                    new Item { Id = 15, ItemId = 5, Name = "Exam", Description = null, IsActivated = true, IsDeleted = false, CreatedBy = 1, CreatedAt = DateTime.Now, UpdatedBy = 1, UpdatedAt = DateTime.Now },
                    new Item { Id = 16, ItemId = 5, Name = "Honey", Description = null, IsActivated = true, IsDeleted = false, CreatedBy = 1, CreatedAt = DateTime.Now, UpdatedBy = 1, UpdatedAt = DateTime.Now },
                    new Item { Id = 17, ItemId = 17, Name = "Mall", Description = null, IsActivated = true, IsDeleted = false, CreatedBy = 1, CreatedAt = DateTime.Now, UpdatedBy = 1, UpdatedAt = DateTime.Now },
                    new Item { Id = 18, ItemId = 17, Name = "Industry", Description = null, IsActivated = true, IsDeleted = false, CreatedBy = 1, CreatedAt = DateTime.Now, UpdatedBy = 1, UpdatedAt = DateTime.Now },
                    new Item { Id = 19, ItemId = 17, Name = "Discussion", Description = null, IsActivated = true, IsDeleted = false, CreatedBy = 1, CreatedAt = DateTime.Now, UpdatedBy = 1, UpdatedAt = DateTime.Now },
                    new Item { Id = 20, ItemId = 6, Name = "Editor", Description = null, IsActivated = true, IsDeleted = false, CreatedBy = 1, CreatedAt = DateTime.Now, UpdatedBy = 1, UpdatedAt = DateTime.Now },
                    new Item { Id = 21, ItemId = 6, Name = "Revenue", Description = null, IsActivated = true, IsDeleted = false, CreatedBy = 1, CreatedAt = DateTime.Now, UpdatedBy = 1, UpdatedAt = DateTime.Now },
                    new Item { Id = 22, ItemId = 6, Name = "Article", Description = null, IsActivated = true, IsDeleted = false, CreatedBy = 1, CreatedAt = DateTime.Now, UpdatedBy = 1, UpdatedAt = DateTime.Now },
                    new Item { Id = 23, ItemId = 6, Name = "Situation", Description = null, IsActivated = true, IsDeleted = false, CreatedBy = 1, CreatedAt = DateTime.Now, UpdatedBy = 1, UpdatedAt = DateTime.Now },
                    new Item { Id = 24, ItemId = 25, Name = "Singer", Description = null, IsActivated = true, IsDeleted = false, CreatedBy = 1, CreatedAt = DateTime.Now, UpdatedBy = 1, UpdatedAt = DateTime.Now },
                    new Item { Id = 25, ItemId = 25, Name = "Feedback", Description = null, IsActivated = true, IsDeleted = false, CreatedBy = 1, CreatedAt = DateTime.Now, UpdatedBy = 1, UpdatedAt = DateTime.Now },
                    new Item { Id = 26, ItemId = 25, Name = "Opportunity", Description = null, IsActivated = true, IsDeleted = false, CreatedBy = 1, CreatedAt = DateTime.Now, UpdatedBy = 1, UpdatedAt = DateTime.Now },
                    new Item { Id = 27, ItemId = 25, Name = "Championship", Description = null, IsActivated = true, IsDeleted = false, CreatedBy = 1, CreatedAt = DateTime.Now, UpdatedBy = 1, UpdatedAt = DateTime.Now }
                }
            );
        }
    }
}
