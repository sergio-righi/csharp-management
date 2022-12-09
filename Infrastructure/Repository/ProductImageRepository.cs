using Domain.Entity;
using Infrastructure.Context;
using Infrastructure.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Repository
{
    public class ProductImageRepository : DomainRepository<ProductImage>, IProductImageRepository
    {
        public ProductImageRepository(DBContext dbContext) : base(dbContext)
        {
        }
    }
}
