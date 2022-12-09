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
    public class RentProductService : BaseService<RentProduct>, IRentProductService
    {
        private readonly IRentProductRepository Repository;

        public RentProductService(IRentProductRepository repository) : base(repository)
        {
            Repository = repository;
        }

        public async Task<ICollection<SProduct>> SList(int rent_id)
        {
            return await Repository.SList(rent_id);
        }

        public async Task<bool> Manage(ICollection<RentProduct> add_modified, ICollection<RentProduct> deleted)
        {
            return await Repository.Manage(add_modified, deleted);
        }
    }
}
