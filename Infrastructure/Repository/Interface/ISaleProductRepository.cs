using Domain.Entity;
using Domain.Entity.Specific;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository.Interface
{
    public interface ISaleProductRepository : IDomainRepository<SaleProduct>
    {
        Task<ICollection<SProduct>> SList(int saleId);
        Task<bool> Manage(ICollection<SaleProduct> add_modified, ICollection<SaleProduct> deleted);
    }
}
