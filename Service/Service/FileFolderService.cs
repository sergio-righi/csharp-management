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
    public class FileFolderService : BaseService<FileFolder>, IFileFolderService
    {
        private readonly IFileFolderRepository Repository;

        public FileFolderService(IFileFolderRepository repository) : base(repository)
        {
            Repository = repository;
        }
    }
}
