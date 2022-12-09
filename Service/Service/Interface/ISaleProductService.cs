using Domain.Entity;
using Domain.Entity.Specific;
using Domain.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Service.Service.Interface
{
    public interface ISaleProductService : IService<SaleProduct>
    {
        Task<ICollection<SProduct>> SList(int saleId);
        Task<bool> Manage(ICollection<SaleProduct> addModified, ICollection<SaleProduct> deleted);
    }
}
