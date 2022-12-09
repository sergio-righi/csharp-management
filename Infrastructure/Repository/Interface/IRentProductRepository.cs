using Domain.Entity;
using Domain.Entity.Specific;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository.Interface
{
    public interface IRentProductRepository : IDomainRepository<RentProduct>
    {
        Task<ICollection<SProduct>> SList(int rentId);
        Task<bool> Manage(ICollection<RentProduct> add_modified, ICollection<RentProduct> deleted);
    }
}
