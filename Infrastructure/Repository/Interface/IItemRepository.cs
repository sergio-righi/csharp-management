using Domain.Entity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository.Interface
{
    public interface IItemRepository : IDomainRepository<Item>
    {
        Task<Item> Find(int id, bool? deleted);
        Task<ICollection<Item>> List(string name, bool? activated, bool? deleted);
    }
}
