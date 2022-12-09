using Domain.Entity;
using Infrastructure.Context;
using Infrastructure.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    public class NaturalPersonRepository : DomainRepository<NaturalPerson>, INaturalPersonRepository
    {
        public NaturalPersonRepository(DBContext dbContext) : base(dbContext)
        {
        }

        public async Task<NaturalPerson> Find(int id)
        {
            var query = (from PP in DbContext.NaturalPeople
                         where PP.Id == id
                         select PP)
                         .Include(x => x.Person).ThenInclude(y => y.Photo)
                         .FirstOrDefault();

            return await Task.FromResult(query);
        }
    }
}
