using Domain.Entity;
using Infrastructure.Context;
using Infrastructure.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    public class GroupRepository : DomainRepository<Group>, IGroupRepository
    {
        public GroupRepository(DBContext dbContext) : base(dbContext)
        {
        }
    }
}
