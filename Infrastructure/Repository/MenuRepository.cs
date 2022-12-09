using Domain.Entity;
using Infrastructure.Context;
using Infrastructure.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Repository
{
    public class MenuRepository : DomainRepository<Menu>, IMenuRepository
    {
        public MenuRepository(DBContext dbContext) : base(dbContext)
        {
        }
    }
}
