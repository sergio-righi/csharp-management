using Domain.Entity;
using Infrastructure.Repository.Interface;
using Service.Service.Base;
using Service.Service.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Service.Service
{
    public class SubgroupService : BaseService<Subgroup>, ISubgroupService
    {
        private readonly ISubgroupRepository Repository;

        public SubgroupService(ISubgroupRepository repository) : base(repository)
        {
            Repository = repository;
        }
    }
}
