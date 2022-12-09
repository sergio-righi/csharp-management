using Domain.Entity;
using Domain.Entity.Generic;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository.Interface
{
    public interface IAddressRepository : IDomainRepository<Address>
    {
        Task<Address> Find(int id, bool? deleted);
        Task<ICollection<Address>> List(int personId, bool? deleted);
        Task<ICollection<GCustomInformation>> GList(int personId, bool? deleted);
    }
}
