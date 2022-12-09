using Domain.Entity;
using Infrastructure.Context;
using Infrastructure.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Tool.Extensions;

namespace Infrastructure.Repository
{
    public class RentRepository : DomainRepository<Rent>, IRentRepository
    {
        public RentRepository(DBContext dbContext) : base(dbContext)
        {
        }
    }
}
