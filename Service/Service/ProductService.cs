using Domain.Entity;
using Domain.Entity.Specific;
using Infrastructure.Repository.Interface;
using Service.Service.Base;
using Service.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Tool.Utilities;

namespace Service.Service
{
    public class ProductService : BaseService<Product>, IProductService
    {
        private readonly IProductRepository Repository;

        public ProductService(IProductRepository repository) : base(repository)
        {
            Repository = repository;
        }

        public async Task<Product> Find(int id, bool deleted)
        {
            return await Repository.Find(id, deleted);
        }

        public async Task<Pagination<SProduct>> SList(string name, EController controler_id, bool? activated, bool? deleted, int page, int count)
        {
            return await Repository.SList(name, controler_id, activated, deleted, page, count);
        }
    }
}
