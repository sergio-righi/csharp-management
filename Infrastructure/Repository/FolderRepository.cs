using Domain.Entity;
using Infrastructure.Context;
using Infrastructure.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace Infrastructure.Repository
{
    public class FolderRepository : DomainRepository<Folder>, IFolderRepository
    {
        public FolderRepository(DBContext dbContext) : base(dbContext)
        {
        }
    }
}
