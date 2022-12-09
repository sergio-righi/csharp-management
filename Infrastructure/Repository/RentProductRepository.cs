using Domain.Entity;
using Domain.Entity.Specific;
using Infrastructure.Context;
using Infrastructure.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Tool.Extensions;

namespace Infrastructure.Repository
{
    public class RentProductRepository : DomainRepository<RentProduct>, IRentProductRepository
    {
        public RentProductRepository(DBContext dbContext) : base(dbContext)
        {
        }

        public async Task<ICollection<SProduct>> SList(int rentId)
        {
            var query = (from RP in DbContext.RentProduct
                         join P in DbContext.Products on RP.ProductId equals P.Id
                         where RP.RentId == rentId
                         select new SProduct
                         {
                             Id = RP.Id,
                             Name = P.Name,
                             RentId = rentId,
                             Count = RP.Count,
                             Value = RP.Value,
                             ProductId = P.Id,
                             Price = P.RentPrice,
                             CalculationId = P.CalculationId
                         })
                         .AsNoTracking()
                         .ToList();

            return await Task.FromResult(query);
        }

        public async Task<bool> Manage(ICollection<RentProduct> addModified, ICollection<RentProduct> deleted)
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
