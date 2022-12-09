using Domain.Entity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository.Interface
{
    public interface IEmailRepository : IDomainRepository<Email>
    {
        Task<Email> Find(int? id, bool? deleted);
    }
}
