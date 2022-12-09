using Domain.Entity;
using Domain.Entity.Specific;
using Infrastructure.Context;
using Infrastructure.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Tool.Extensions;
using System.Linq;

namespace Infrastructure.Repository
{
    public class SaleProductRepository : DomainRepository<SaleProduct>, ISaleProductRepository
    {
        public SaleProductRepository(DBContext dbContext) : base(dbContext)
        {
        }

        public async Task<ICollection<SProduct>> SList(int saleId)
        {
            var query = (from SP in DbContext.SaleProduct
                         join P in DbContext.Products on SP.ProductId equals P.Id
                         where SP.SaleId == saleId
                         select new SProduct
                         {
                             Id = SP.Id,
                             Name = P.Name,
                             RentId = saleId,
                             Width = SP.Width,
                             Count = SP.Count,
                             Value = SP.Value,
                             ProductId = P.Id,
                             Height = SP.Height,
                             Price = P.SalePrice,
                             CalculationId = P.CalculationId
                         })
                         .AsNoTracking()
                         .ToList();

            return await Task.FromResult(query);
        }

        public async Task<bool> Manage(ICollection<SaleProduct> addModified, ICollection<SaleProduct> deleted)
        {
            using (IDbContextTransaction transaction = DbContext.Database.BeginTransaction())
            {
                try
                {
                    foreach (var item in addModified)
                    {
                        if (item.Id.IsZero())
                        {
                            DbContext.Entry(item).State = EntityState.Added;
                        }
                        else
                        {
                            DbContext.Entry(item).State = EntityState.Modified;
                        }
                    }

                    foreach (var item in deleted)
                    {
                        DbContext.Entry(item).State = EntityState.Deleted;
                    }

                    await DbContext.SaveChangesAsync();

                    transaction.Commit();

                    return true;
                }
                catch (Exception)
                {
                    transaction.Rollback();

                    return false;
                }
            }
        }
    }
}
