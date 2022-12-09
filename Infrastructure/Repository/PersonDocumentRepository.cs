using Domain.Entity;
using Infrastructure.Context;
using Infrastructure.Repository.Interface;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository
{
    public class PersonDocumentRepository : DomainRepository<PersonDocument>, IPersonDocumentRepository
    {
        public PersonDocumentRepository(DBContext dbContext) : base(dbContext)
        {
        }

        public async Task<ICollection<PersonDocument>> List(int personId)
        {
            var query = (from PD in DbContext.PersonDoument
                         where PD.PersonId == personId
                         select PD)
                         .AsNoTracking()
                         .ToList();

            return await Task.FromResult(query);
        }
    }
}
