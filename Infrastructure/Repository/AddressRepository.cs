using Domain.Entity;
using Infrastructure.Context;
using Infrastructure.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Domain.Entity.Generic;

namespace Infrastructure.Repository
{
    public class AddressRepository : DomainRepository<Address>, IAddressRepository
    {
        public AddressRepository(DBContext dbContext) : base(dbContext)
        {
        }

        public async Task<Address> Find(int id, bool? deleted)
        {
            var query = (from A in DbContext.Addresses
                         where A.Id == id && A.IsDeleted == (deleted ?? A.IsDeleted)
                         select A)
                         .Include(x => x.City)
                         .FirstOrDefault();

            return await Task.FromResult(query);
        }

        public async Task<ICollection<Address>> List(int personId, bool? deleted)
        {
            var query = (from A in DbContext.Addresses
                         where A.PersonId == personId && A.IsDeleted == (deleted ?? A.IsDeleted)
                         select A)
                         .AsNoTracking()
                         .ToList();

            return await Task.FromResult(query);
        }

        public async Task<ICollection<GCustomInformation>> GList(int personId, bool? deleted)
        {
            var query = (from A in DbContext.Addresses
                         .Include(x => x.City)
                         .ThenInclude(x => x.State)
                         .ThenInclude(x => x.Country)
                         where A.PersonId == personId && A.IsDeleted == (deleted ?? A.IsDeleted)
                         select new GCustomInformation()
                         {
                             Id = A.Id,
                             Name = $"{A.Street}, {A.Number} - {A.Zipcode} - {A.City.Name} ({A.City.State.Abbreviation}) - {A.City.Country.Name}"
                         })
                         .AsNoTracking()
                         .ToList();

            return await Task.FromResult(query);
        }
    }
}
