using Domain.Entity;
using Infrastructure.Repository.Interface;
using Service.Service.Base;
using Service.Service.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Service
{
    public class ProductImageService : BaseService<ProductImage>, IProductImageService
    {
        private readonly IProductImageRepository Repository;

        public ProductImageService(IProductImageRepository repository) : base(repository)
        {
            Repository = repository;
        }
    }
}
