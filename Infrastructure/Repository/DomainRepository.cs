using Domain.Interface;
using Infrastructure.Context;
using Infrastructure.Repository.Base;
using Infrastructure.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Repository
{
    public class DomainRepository<T> : BaseRepository<T>, IDomainRepository<T> where T : class, IEntity
    {
        protected DomainRepository(DBContext dbContext) : base(dbContext)
        {
        }
    }
}
