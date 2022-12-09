using Domain.Entity;
using Domain.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Service.Service.Interface
{
    public interface IItemService : IService<Item>
    {
        Task<Item> Find(int id, bool? deleted);
        Task<ICollection<Item>> List(string name, bool? activated, bool? deleted);
    }
}
