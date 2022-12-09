using Domain.Entity;
using Infrastructure.Context;
using Infrastructure.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    public class SubgroupRepository : DomainRepository<Subgroup>, ISubgroupRepository
    {
        public SubgroupRepository(DBContext dbContext) : base(dbContext)
        {
        }
    }
}
