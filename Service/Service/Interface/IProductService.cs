using Domain.Entity;
using Domain.Entity.Specific;
using Domain.Interface;
using Infrastructure.Repository.Interface;
using Service.Service.Base;
using Service.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Tool.Utilities;

namespace Service.Service.Interface
{
    public interface IProductService : IService<Product>
    {
        Task<Product> Find(int id, bool deleted);
        Task<Pagination<SProduct>> SList(string name, EController controllerId, bool? activated, bool? deleted, int page, int count);
    }
}
