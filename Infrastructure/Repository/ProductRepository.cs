

using Domain.Entity;
using Domain.Entity.Specific;
using System.Threading.Tasks;
using Tool.Utilities;
using System.Linq;
using Infrastructure.Context;
using Infrastructure.Repository.Interface;
using System;
using System.Linq.Expressions;

namespace Infrastructure.Repository
{
    public class ProductRepository : DomainRepository<Product>, IProductRepository
    {
        public ProductRepository(DBContext dbContext) : base(dbContext)
        {
        }

        public async Task<Product> Find(int id, bool deleted)
        {
            var query = (from P in DbContext.Products
                         where P.Id == id && P.IsDeleted == deleted
                         select P)
                         .FirstOrDefault();

            return await Task.FromResult(query);
        }

        public async Task<Pagination<SProduct>> SList(string name, EController controllerId, bool? activated, bool? deleted, int page, int count)
        {
            var query = (from P in DbContext.Products
                         where
                            (!string.IsNullOrWhiteSpace(name) ? P.Name.Contains(name) : true) && P.IsActivated == (activated ?? P.IsActivated) && P.IsDeleted == (deleted ?? P.IsDeleted) && (controllerId == EController.Rent ? P.RentPrice.HasValue == true : P.SalePrice.HasValue == true)
                         select new SProduct
                         {
                             Name = P.Name,
                             ProductId = P.Id,
                             Count = P.Count ?? 0,
                             CalculationId = P.CalculationId,
                             Price = controllerId == EController.Rent ? P.RentPrice : P.SalePrice
                         })
                         .OrderBy(x => x.Name)
                         .Skip((page - 1) * count)
                         .Take(count)
                         .ToList();

            var total = (from P in DbContext.Products
                         where
                           (!string.IsNullOrWhiteSpace(name) ? P.Name.Contains(name) : true) && P.IsActivated == (activated ?? P.IsActivated) && P.IsDeleted == (deleted ?? P.IsDeleted) && (controllerId == EController.Rent ? P.RentPrice.HasValue == true : P.SalePrice.HasValue == true)
                         select P.Id)
                         .Count();

            query = await Task.FromResult(query);

            return new Pagination<SProduct>(query, total, page, count);
        }
    }
}
