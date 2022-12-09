using Domain.Entity;
using Domain.Interface;
using Infrastructure.Repository.Interface;
using Service.Service.Base;
using Service.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Service
{
    public class ItemService : BaseService<Item>, IItemService
    {
        private readonly IItemRepository Repository;

        public ItemService(IItemRepository repository) : base(repository)
        {
            Repository = repository;
        }

        public async Task<Item> Find(int id, bool? deleted)
        {
            return await Repository.Find(id, deleted);
        }

        public async Task<ICollection<Item>> List(string name, bool? activated, bool? deleted)
        {
            return await Repository.List(name, activated, deleted);
        }
    }
}
