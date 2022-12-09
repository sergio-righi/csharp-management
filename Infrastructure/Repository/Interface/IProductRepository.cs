using Domain.Entity;
using Domain.Entity.Specific;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Tool.Utilities;

namespace Infrastructure.Repository.Interface
{
    public interface IProductRepository : IDomainRepository<Product>
    {
        Task<Product> Find(int id, bool deleted);
        Task<Pagination<SProduct>> SList(string name, EController controllerId, bool? activated, bool? deleted, int page, int count);
    }
}
