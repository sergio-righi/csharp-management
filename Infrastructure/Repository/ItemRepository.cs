using Domain.Entity;
using Infrastructure.Context;
using Infrastructure.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository
{
    public class ItemRepository : DomainRepository<Item>, IItemRepository
    {
        public ItemRepository(DBContext dbContext) : base(dbContext)
        {
        }

        public async Task<Item> Find(int id, bool? deleted)
        {
            var query = (from I in DbContext.Items
                         where I.Id == id && I.IsDeleted == (deleted ?? I.IsDeleted)
                         select I)
                         .FirstOrDefault();

            return await Task.FromResult(query);
        }

        public async Task<ICollection<Item>> List(string name, bool? activated, bool? deleted)
        {
            var query = (from I in DbContext.Items
                         where
                           (!string.IsNullOrWhiteSpace(name) ? I.Name.Contains(name) : true) && I.IsActivated == (activated ?? I.IsActivated) && I.IsDeleted == (deleted ?? I.IsDeleted)
                         orderby I.Name
                         select I)
                         .Include(x => x.Parent)
                         .AsNoTracking()
                         .ToList();

            return await Task.FromResult(query);
        }
    }
}