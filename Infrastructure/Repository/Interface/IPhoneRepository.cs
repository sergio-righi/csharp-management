using Domain.Entity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository.Interface
{
    public interface IPhoneRepository : IDomainRepository<Phone>
    {
        Task<Phone> Find(int? id, bool? deleted);
    }
}
