using Domain.Entity;
using Infrastructure.Context;
using Infrastructure.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Infrastructure.Repository
{
    public class InstallmentRepository : DomainRepository<Installment>, IInstallmentRepository
    {
        public InstallmentRepository(DBContext dbContext) : base(dbContext)
        {
        }

        public async Task<Installment> Find(int id)
        {
            var query = (from I in DbContext.Installments
                         where I.Id == id
                         select I)
                         .Include(x => x.Dates)
                         .FirstOrDefault();

            return await Task.FromResult(query);
        }

        public async Task<int> Count(int installmentId)
        {
            var query = (from I in DbContext.Installments
                         join II in DbContext.InstallmentInfo on I.Id equals II.InstallmentId
                         where I.Id == installmentId
                         select II)
                         .Count();

            return await Task.FromResult(query);
        }

        public async Task<bool> Manage(Installment installment)
        {
            using (IDbContextTransaction transaction = DbContext.Database.BeginTransaction())
            {
                try
                {
                    DbContext.Entry(installment).State = EntityState.Modified;

                    foreach (var item in installment.Dates)
                    {
                        item.UpdatedAt = installment.UpdatedAt;
                        item.UpdatedBy = installment.UpdatedBy;

                        if (item.NotIsDeleted)
                        {
                            DbContext.Entry(item).State = EntityState.Deleted;
                        }
                        else if (item.Id == 0)
                        {
                            item.CreatedAt = installment.UpdatedAt;
                            item.CreatedBy = installment.UpdatedBy;

                            DbContext.Entry(item).State = EntityState.Added;
                        }
                        else
                        {
                            DbContext.Entry(item).Property(x => x.CreatedBy).IsModified = false;
                            DbContext.Entry(item).Property(x => x.CreatedAt).IsModified = false;

                            DbContext.Entry(item).State = EntityState.Modified;
                        }
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
