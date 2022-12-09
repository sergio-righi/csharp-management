using Domain.Entity;
using Infrastructure.Context;
using Infrastructure.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository
{
    public class EmailRepository : DomainRepository<Email>, IEmailRepository
    {
        public EmailRepository(DBContext dbContext) : base(dbContext)
        {
        }

        public async Task<Email> Find(int? id, bool? deleted)
        {
            var query = (from E in DbContext.Emails
                         where E.Id == id && E.IsDeleted == (deleted ?? E.IsDeleted)
                         select E)
                         .FirstOrDefault();

            return await Task.FromResult(query);
        }
    }
}
