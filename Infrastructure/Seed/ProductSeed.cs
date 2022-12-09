using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Tool.Utilities;

namespace Infrastructure.Seed
{
    public static class ProductSeed
    {
        public static void SeedProducts(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().HasData(
                new[] {
                    new Product { Id = 1, ItemId = 1, Name = "Fantasy", Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit", CalculationId = ECalculation.Unit, Count = 99, PurchasePrice = 100, SalePrice = null, RentPrice = 50, Width = null, Height = null, Weight = null, SizeId = ESize.None, ColorId = EColor.None, MaterialId = EMaterial.None, ShapeId = EShape.None, IsActivated = true, IsDeleted = false, CreatedBy = 1, CreatedAt = DateTime.Now, UpdatedBy = 1, UpdatedAt = DateTime.Now },
                    new Product { Id = 2, ItemId = 1, Name = "Donor", Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit", CalculationId = ECalculation.Unit, Count = 99, PurchasePrice = 100, SalePrice = null, RentPrice = 75, Width = null, Height = null, Weight = null, SizeId = ESize.None, ColorId = EColor.None, MaterialId = EMaterial.None, ShapeId = EShape.None, IsActivated = true, IsDeleted = false, CreatedBy = 1, CreatedAt = DateTime.Now, UpdatedBy = 1, UpdatedAt = DateTime.Now },
                    new Product { Id = 3, ItemId = 1, Name = "Plaintiff", Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit", CalculationId = ECalculation.Unit, Count = 99, PurchasePrice = 200, SalePrice = null, RentPrice = 150, Width = null, Height = null, Weight = null, SizeId = ESize.None, ColorId = EColor.None, MaterialId = EMaterial.None, ShapeId = EShape.None, IsActivated = true, IsDeleted = false, CreatedBy = 1, CreatedAt = DateTime.Now, UpdatedBy = 1, UpdatedAt = DateTime.Now },
                    new Product { Id = 4, ItemId = 1, Name = "Forestry", Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit", CalculationId = ECalculation.Unit, Count = 99, PurchasePrice = 500, SalePrice = null, RentPrice = 200, Width = null, Height = null, Weight = null, SizeId = ESize.None, ColorId = EColor.None, MaterialId = EMaterial.None, ShapeId = EShape.None, IsActivated = true, IsDeleted = false, CreatedBy = 1, CreatedAt = DateTime.Now, UpdatedBy = 1, UpdatedAt = DateTime.Now },
                    new Product { Id = 5, ItemId = 1, Name = "Glimpse", Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit", CalculationId = ECalculation.Unit, Count = 99, PurchasePrice = 1000, SalePrice = null, RentPrice = 400, Width = null, Height = null, Weight = null, SizeId = ESize.None, ColorId = EColor.None, MaterialId = EMaterial.None, ShapeId = EShape.None, IsActivated = true, IsDeleted = false, CreatedBy = 1, CreatedAt = DateTime.Now, UpdatedBy = 1, UpdatedAt = DateTime.Now },
                    new Product { Id = 6, ItemId = 4, Name = "Unaware", Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit", CalculationId = ECalculation.Unit, Count = 99, PurchasePrice = 300, SalePrice = 500, RentPrice = null, Width = null, Height = null, Weight = null, SizeId = ESize.None, ColorId = EColor.None, MaterialId = EMaterial.None, ShapeId = EShape.None, IsActivated = true, IsDeleted = false, CreatedBy = 1, CreatedAt = DateTime.Now, UpdatedBy = 1, UpdatedAt = DateTime.Now },
                    new Product { Id = 7, ItemId = 4, Name = "Reservoir", Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit", CalculationId = ECalculation.SquareMeter, Count = 99, PurchasePrice = 200, SalePrice = 350, RentPrice = null, Width = null, Height = null, Weight = null, SizeId = ESize.None, ColorId = EColor.None, MaterialId = EMaterial.None, ShapeId = EShape.None, IsActivated = true, IsDeleted = false, CreatedBy = 1, CreatedAt = DateTime.Now, UpdatedBy = 1, UpdatedAt = DateTime.Now },
                    new Product { Id = 8, ItemId = 4, Name = "Overwhelm", Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit", CalculationId = ECalculation.SquareMeter, Count = 99, PurchasePrice = 150, SalePrice = 300, RentPrice = null, Width = null, Height = null, Weight = null, SizeId = ESize.None, ColorId = EColor.None, MaterialId = EMaterial.None, ShapeId = EShape.None, IsActivated = true, IsDeleted = false, CreatedBy = 1, CreatedAt = DateTime.Now, UpdatedBy = 1, UpdatedAt = DateTime.Now },
                    new Product { Id = 9, ItemId = 4, Name = "Technology", Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit", CalculationId = ECalculation.SquareMeter, Count = 99, PurchasePrice = 250, SalePrice = 400, RentPrice = null, Width = null, Height = null, Weight = null, SizeId = ESize.None, ColorId = EColor.None, MaterialId = EMaterial.None, ShapeId = EShape.None, IsActivated = true, IsDeleted = false, CreatedBy = 1, CreatedAt = DateTime.Now, UpdatedBy = 1, UpdatedAt = DateTime.Now },
                    new Product { Id = 10, ItemId = 4, Name = "Spokesperson", Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit", CalculationId = ECalculation.SquareMeter, Count = 99, PurchasePrice = 600, SalePrice = 750, RentPrice = null, Width = null, Height = null, Weight = null, SizeId = ESize.None, ColorId = EColor.None, MaterialId = EMaterial.None, ShapeId = EShape.None, IsActivated = true, IsDeleted = false, CreatedBy = 1, CreatedAt = DateTime.Now, UpdatedBy = 1, UpdatedAt = DateTime.Now }
                }
            );
        }
    }
}
