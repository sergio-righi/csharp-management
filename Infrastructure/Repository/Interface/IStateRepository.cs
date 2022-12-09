using Domain.Entity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository.Interface
{
    public interface IStateRepository : IDomainRepository<State>
    {
        Task<ICollection<State>> List(bool? activated);
        Task<ICollection<State>> List(int countryId, bool? activated);
    }
}
