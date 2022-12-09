using Domain.Entity;
using Domain.Entity.Specific;
using Domain.Interface;
using Infrastructure.Repository.Interface;
using Service.Service.Base;
using Service.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Service.Interface
{
    public interface IRentProductService : IService<RentProduct>
    {
        Task<ICollection<SProduct>> SList(int rentId);
        Task<bool> Manage(ICollection<RentProduct> addModified, ICollection<RentProduct> deleted);
    }
}
