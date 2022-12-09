using Domain.Entity;
using Domain.Entity.Specific;
using Infrastructure.Repository.Interface;
using Service.Service.Base;
using Service.Service.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Service.Service
{
    public class SaleProductService : BaseService<SaleProduct>, ISaleProductService
    {
        private readonly ISaleProductRepository Repository;

        public SaleProductService(ISaleProductRepository repository) : base(repository)
        {
            Repository = repository;
        }

        public async Task<ICollection<SProduct>> SList(int sale_id)
        {
            return await Repository.SList(sale_id);
        }

        public async Task<bool> Manage(ICollection<SaleProduct> add_modified, ICollection<SaleProduct> deleted)
        {
            return await Repository.Manage(add_modified, deleted);
        }
    }
}
