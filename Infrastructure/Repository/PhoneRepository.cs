using Domain.Entity;
using Infrastructure.Context;
using Infrastructure.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    public class PhoneRepository : DomainRepository<Phone>, IPhoneRepository
    {
        public PhoneRepository(DBContext dbContext) : base(dbContext)
        {
        }

        public async Task<Phone> Find(int? id, bool? deleted)
        {
            var query = (from P in DbContext.Phones
                         where P.Id == id && P.IsDeleted == (deleted ?? P.IsDeleted)
                         select P)
                         .FirstOrDefault();

            return await Task.FromResult(query);
        }
    }
}
