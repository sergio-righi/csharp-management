using Domain.Entity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository.Interface
{
    public interface ICountryRepository : IDomainRepository<Country>
    {
        Task<ICollection<Country>> List(bool? activated);
    }
}
