using Domain.Base;
using Domain.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Repository.Interface
{
    public interface IDomainRepository<T> : IRepository<T> where T : class, IEntity
    {
    }
}
