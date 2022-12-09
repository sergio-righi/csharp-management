using Domain.Entity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository.Interface
{
    public interface ICityRepository : IDomainRepository<City>
    {
        Task<ICollection<City>> List(int? countryId, int? stateId, bool? activated);
    }
}
