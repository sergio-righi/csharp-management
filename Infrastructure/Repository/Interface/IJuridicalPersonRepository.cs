using Domain.Entity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository.Interface
{
    public interface IJuridicalPersonRepository : IDomainRepository<JuridicalPerson>
    {
        Task<JuridicalPerson> Find(int id);
    }
}
