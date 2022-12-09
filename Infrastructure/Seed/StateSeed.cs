using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Seed
{
    public static class StateSeed
    {
        public static void SeedStates(this ModelBuilder modelBuilder)
        {
            /// brazil
            modelBuilder.Entity<State>().HasData(
                new State() { Id = 1, CountryId = 30, Name = "Acre", PhoneCode = "", Abbreviation = "AC", IsActivated = true, CreatedBy = 1, CreatedAt = DateTime.Now, UpdatedBy = 1, UpdatedAt = DateTime.Now },
                new State() { Id = 2, CountryId = 30, Name = "Alagoas", PhoneCode = "", Abbreviation = "AL", IsActivated = true, CreatedBy = 1, CreatedAt = DateTime.Now, UpdatedBy = 1, UpdatedAt = DateTime.Now },
                new State() { Id = 3, CountryId = 30, Name = "Amazonas", PhoneCode = "", Abbreviation = "AM", IsActivated = true, CreatedBy = 1, CreatedAt = DateTime.Now, UpdatedBy = 1, UpdatedAt = DateTime.Now },
                new State() { Id = 4, CountryId = 30, Name = "Amapá", PhoneCode = "", Abbreviation = "AP", IsActivated = true, CreatedBy = 1, CreatedAt = DateTime.Now, UpdatedBy = 1, UpdatedAt = DateTime.Now },
                new State() { Id = 5, CountryId = 30, Name = "Bahia", PhoneCode = "", Abbreviation = "BA", IsActivated = true, CreatedBy = 1, CreatedAt = DateTime.Now, UpdatedBy = 1, UpdatedAt = DateTime.Now },
                new State() { Id = 6, CountryId = 30, Name = "Ceará", PhoneCode = "", Abbreviation = "CE", IsActivated = true, CreatedBy = 1, CreatedAt = DateTime.Now, UpdatedBy = 1, UpdatedAt = DateTime.Now },
                new State() { Id = 7, CountryId = 30, Name = "Distrito Federal", PhoneCode = "", Abbreviation = "DF", IsActivated = true, CreatedBy = 1, CreatedAt = DateTime.Now, UpdatedBy = 1, UpdatedAt = DateTime.Now },
                new State() { Id = 8, CountryId = 30, Name = "Espírito Santo", PhoneCode = "", Abbreviation = "ES", IsActivated = true, CreatedBy = 1, CreatedAt = DateTime.Now, UpdatedBy = 1, UpdatedAt = DateTime.Now },
                new State() { Id = 9, CountryId = 30, Name = "Goiás", PhoneCode = "", Abbreviation = "GO", IsActivated = true, CreatedBy = 1, CreatedAt = DateTime.Now, UpdatedBy = 1, UpdatedAt = DateTime.Now },
                new State() { Id = 10, CountryId = 30, Name = "Maranhão", PhoneCode = "", Abbreviation = "MA", IsActivated = true, CreatedBy = 1, CreatedAt = DateTime.Now, UpdatedBy = 1, UpdatedAt = DateTime.Now },
                new State() { Id = 11, CountryId = 30, Name = "Minas Gerais", PhoneCode = "", Abbreviation = "MG", IsActivated = true, CreatedBy = 1, CreatedAt = DateTime.Now, UpdatedBy = 1, UpdatedAt = DateTime.Now },
                new State() { Id = 12, CountryId = 30, Name = "Mato Grosso do Sul", PhoneCode = "", Abbreviation = "MS", IsActivated = true, CreatedBy = 1, CreatedAt = DateTime.Now, UpdatedBy = 1, UpdatedAt = DateTime.Now },
                new State() { Id = 13, CountryId = 30, Name = "Mato Grosso", PhoneCode = "", Abbreviation = "MT", IsActivated = true, CreatedBy = 1, CreatedAt = DateTime.Now, UpdatedBy = 1, UpdatedAt = DateTime.Now },
                new State() { Id = 14, CountryId = 30, Name = "Pará", PhoneCode = "", Abbreviation = "PA", IsActivated = true, CreatedBy = 1, CreatedAt = DateTime.Now, UpdatedBy = 1, UpdatedAt = DateTime.Now },
                new State() { Id = 15, CountryId = 30, Name = "Paraíba", PhoneCode = "", Abbreviation = "PB", IsActivated = true, CreatedBy = 1, CreatedAt = DateTime.Now, UpdatedBy = 1, UpdatedAt = DateTime.Now },
                new State() { Id = 16, CountryId = 30, Name = "Pernambuco", PhoneCode = "", Abbreviation = "PE", IsActivated = true, CreatedBy = 1, CreatedAt = DateTime.Now, UpdatedBy = 1, UpdatedAt = DateTime.Now },
                new State() { Id = 17, CountryId = 30, Name = "Piauí", PhoneCode = "", Abbreviation = "PI", IsActivated = true, CreatedBy = 1, CreatedAt = DateTime.Now, UpdatedBy = 1, UpdatedAt = DateTime.Now },
                new State() { Id = 18, CountryId = 30, Name = "Paraná", PhoneCode = "", Abbreviation = "PR", IsActivated = true, CreatedBy = 1, CreatedAt = DateTime.Now, UpdatedBy = 1, UpdatedAt = DateTime.Now },
                new State() { Id = 19, CountryId = 30, Name = "Rio de Janeiro", PhoneCode = "", Abbreviation = "RJ", IsActivated = true, CreatedBy = 1, CreatedAt = DateTime.Now, UpdatedBy = 1, UpdatedAt = DateTime.Now },
                new State() { Id = 20, CountryId = 30, Name = "Rio Grande do Norte", PhoneCode = "", Abbreviation = "RN", IsActivated = true, CreatedBy = 1, CreatedAt = DateTime.Now, UpdatedBy = 1, UpdatedAt = DateTime.Now },
                new State() { Id = 21, CountryId = 30, Name = "Rondônia", PhoneCode = "", Abbreviation = "RO", IsActivated = true, CreatedBy = 1, CreatedAt = DateTime.Now, UpdatedBy = 1, UpdatedAt = DateTime.Now },
                new State() { Id = 22, CountryId = 30, Name = "Roraima", PhoneCode = "", Abbreviation = "RR", IsActivated = true, CreatedBy = 1, CreatedAt = DateTime.Now, UpdatedBy = 1, UpdatedAt = DateTime.Now },
                new State() { Id = 23, CountryId = 30, Name = "Rio Grande do Sul", PhoneCode = "", Abbreviation = "RS", IsActivated = true, CreatedBy = 1, CreatedAt = DateTime.Now, UpdatedBy = 1, UpdatedAt = DateTime.Now },
                new State() { Id = 24, CountryId = 30, Name = "Santa Catarina", PhoneCode = "", Abbreviation = "SC", IsActivated = true, CreatedBy = 1, CreatedAt = DateTime.Now, UpdatedBy = 1, UpdatedAt = DateTime.Now },
                new State() { Id = 25, CountryId = 30, Name = "Sergipe", PhoneCode = "", Abbreviation = "SE", IsActivated = true, CreatedBy = 1, CreatedAt = DateTime.Now, UpdatedBy = 1, UpdatedAt = DateTime.Now },
                new State() { Id = 26, CountryId = 30, Name = "São Paulo", PhoneCode = "", Abbreviation = "SP", IsActivated = true, CreatedBy = 1, CreatedAt = DateTime.Now, UpdatedBy = 1, UpdatedAt = DateTime.Now },
                new State() { Id = 27, CountryId = 30, Name = "Tocantins", PhoneCode = "", Abbreviation = "TO", IsActivated = true, CreatedBy = 1, CreatedAt = DateTime.Now, UpdatedBy = 1, UpdatedAt = DateTime.Now }
            );

            /// canada
            modelBuilder.Entity<State>().HasData(
                new State() { Id = 28, CountryId = 38, Name = "Alberta", PhoneCode = "", Abbreviation = "AB", IsActivated = true, CreatedBy = 1, CreatedAt = DateTime.Now, UpdatedBy = 1, UpdatedAt = DateTime.Now },
                new State() { Id = 29, CountryId = 38, Name = "British Columbia", PhoneCode = "", Abbreviation = "BC", IsActivated = true, CreatedBy = 1, CreatedAt = DateTime.Now, UpdatedBy = 1, UpdatedAt = DateTime.Now },
                new State() { Id = 30, CountryId = 38, Name = "Manitoba", PhoneCode = "", Abbreviation = "MB", IsActivated = true, CreatedBy = 1, CreatedAt = DateTime.Now, UpdatedBy = 1, UpdatedAt = DateTime.Now },
                new State() { Id = 31, CountryId = 38, Name = "New Brunswick", PhoneCode = "", Abbreviation = "NB", IsActivated = true, CreatedBy = 1, CreatedAt = DateTime.Now, UpdatedBy = 1, UpdatedAt = DateTime.Now },
                new State() { Id = 32, CountryId = 38, Name = "Newfoundland and Labrador", PhoneCode = "", Abbreviation = "NL", IsActivated = true, CreatedBy = 1, CreatedAt = DateTime.Now, UpdatedBy = 1, UpdatedAt = DateTime.Now },
                new State() { Id = 33, CountryId = 38, Name = "Northwest Territories", PhoneCode = "", Abbreviation = "NT", IsActivated = true, CreatedBy = 1, CreatedAt = DateTime.Now, UpdatedBy = 1, UpdatedAt = DateTime.Now },
                new State() { Id = 34, CountryId = 38, Name = "Nova Scotia", PhoneCode = "", Abbreviation = "NS", IsActivated = true, CreatedBy = 1, CreatedAt = DateTime.Now, UpdatedBy = 1, UpdatedAt = DateTime.Now },
                new State() { Id = 35, CountryId = 38, Name = "Nunavut", PhoneCode = "", Abbreviation = "NU", IsActivated = true, CreatedBy = 1, CreatedAt = DateTime.Now, UpdatedBy = 1, UpdatedAt = DateTime.Now },
                new State() { Id = 36, CountryId = 38, Name = "Ontario", PhoneCode = "", Abbreviation = "ON", IsActivated = true, CreatedBy = 1, CreatedAt = DateTime.Now, UpdatedBy = 1, UpdatedAt = DateTime.Now },
                new State() { Id = 37, CountryId = 38, Name = "Prince Edward Island", PhoneCode = "", Abbreviation = "PE", IsActivated = true, CreatedBy = 1, CreatedAt = DateTime.Now, UpdatedBy = 1, UpdatedAt = DateTime.Now },
                new State() { Id = 38, CountryId = 38, Name = "Quebec", PhoneCode = "", Abbreviation = "QC", IsActivated = true, CreatedBy = 1, CreatedAt = DateTime.Now, UpdatedBy = 1, UpdatedAt = DateTime.Now },
                new State() { Id = 39, CountryId = 38, Name = "Saskatchewan", PhoneCode = "", Abbreviation = "SK", IsActivated = true, CreatedBy = 1, CreatedAt = DateTime.Now, UpdatedBy = 1, UpdatedAt = DateTime.Now },
                new State() { Id = 40, CountryId = 38, Name = "Yukon", PhoneCode = "", Abbreviation = "YT", IsActivated = true, CreatedBy = 1, CreatedAt = DateTime.Now, UpdatedBy = 1, UpdatedAt = DateTime.Now }
            );
        }
    }
}
