using Domain.Entity;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Tool.Utilities;

namespace Infrastructure.Seed
{
    public static class PersonSeed
    {
        public static void SeedPeople(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Person>().HasData(
                new[]
                {
                    new Person {
                        Id = 1,
                        TypeId = EPerson.Natural,
                        IsActivated = true,
                        IsDeleted = false,
                        Login = "sergiorighi",
                        Password = Cryptography.GetEncrypted("Qwer12#4"),
                        CreatedBy = 1,
                        CreatedAt = DateTime.Now,
                        UpdatedBy = 1,
                        UpdatedAt = DateTime.Now
                    }
                }
            );

            modelBuilder.Entity<NaturalPerson>().HasData(
                new[]
                {
                    new NaturalPerson
                    {
                        Id = 1,
                        GenderId = EGender.Male,
                        GivenName = "Sérgio",
                        Surname = "Righi",
                        FullName = "Sérgio Righi",
                        Birthday = new DateTime(1994, 03, 02),
                        CreatedBy = 1,
                        CreatedAt = DateTime.Now,
                        UpdatedBy = 1,
                        UpdatedAt = DateTime.Now
                    }
                }
            );

            modelBuilder.Entity<PersonRole>().HasData(
                new[]
                {
                    new PersonRole {
                        Id = 1,
                        PersonId = 1,
                        RoleId = ERole.IT,
                        StartDate = DateTime.Now,
                        EndDate = DateTime.Now,
                        CreatedBy = 1,
                        CreatedAt = DateTime.Now,
                        UpdatedBy = 1,
                        UpdatedAt = DateTime.Now
                    }
                }
            );

            modelBuilder.Entity<Address>().HasData(
                new[]
                {
                    new Address {
                        Id = 1,
                        PersonId = 1,
                        CityId = 4214,
                        Zipcode = "97070500",
                        Street = "Rua Justino Couto",
                        Neighborhood = "Patronato",
                        Number = 535,
                        IsDeleted = false,
                        CreatedBy = 1,
                        CreatedAt = DateTime.Now,
                        UpdatedBy = 1,
                        UpdatedAt = DateTime.Now
                    }
                }
            );

            modelBuilder.Entity<PersonDocument>().HasData(
                new[]
                {
                    new PersonDocument {
                        Id = 1,
                        PersonId = 1,
                        StateId = null,
                        CountryId = null,
                        DocumentId = EDocument.Id,
                        Number = "01384802061",
                        IssueDate = null,
                        ExpiryDate = null,
                        CreatedBy = 1,
                        CreatedAt = DateTime.Now,
                        UpdatedBy = 1,
                        UpdatedAt = DateTime.Now
                    }
                }
            );
        }
    }
}
