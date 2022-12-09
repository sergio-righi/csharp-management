using Domain.Entity;
using Infrastructure.Context;
using Infrastructure.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Repository
{
    public class BillRepository : DomainRepository<Bill>, IBillRepository
    {
        public BillRepository(DBContext dbContext) : base(dbContext)
        {
        }
    }
}
