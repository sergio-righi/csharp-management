using Domain.Entity;
using Infrastructure.Context;
using Infrastructure.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Repository
{
    public class FileFolderRepository : DomainRepository<FileFolder>, IFileFolderRepository
    {
        public FileFolderRepository(DBContext dbContext) : base(dbContext)
        {
        }
    }
}
