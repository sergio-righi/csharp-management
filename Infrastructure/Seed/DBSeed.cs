using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Seed
{
    public static class DBSeed
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.SeedPeople();

            modelBuilder.SeedGroups();
            modelBuilder.SeedSubgroups();

            modelBuilder.SeedCountries();
            modelBuilder.SeedStates();
            modelBuilder.SeedCitites();

            modelBuilder.SeedItems();
            modelBuilder.SeedProducts();
        }
    }
}
