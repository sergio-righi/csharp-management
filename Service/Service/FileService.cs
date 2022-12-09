using Domain.Entity;
using Domain.Interface;
using Infrastructure.Repository.Interface;
using Service.Service.Base;
using Service.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Tool.Utilities;

namespace Service.Service
{
    public class FileService : BaseService<File>, IFileService
    {
        private readonly IFileRepository Repository;

        public FileService(IFileRepository repository) : base(repository)
        {
            Repository = repository;
        }

        public async Task<File> Find(string key, bool? activated, bool? deleted)
        {
            return await Repository.Find(key, activated, deleted);
        }

        public async Task<File> FindPhoto(int personId)
        {
            return await Repository.FindPhoto(personId);
        }

        public async Task<ICollection<string>> ListExtension(int? personId, bool? activated, bool? deleted)
        {
            return await Repository.ListExtension(personId, activated, deleted);
        }

        public async Task<ICollection<File>> List(string name, int? category, int? personId, int? folderId, bool? activated, bool? deleted, Expression<Func<File, object>> order, EDirection direction, int count)
        {
            return await Repository.List(name, category, personId, folderId, activated, deleted, order, direction, count);
        }

        public async Task<Pagination<File>> List(string name, int? category, int? personId, int? folderId, bool? activated, bool? deleted, Expression<Func<File, object>> order, EDirection direction, int page, int count)
        {
            return await Repository.List(name, category, personId, folderId, activated, deleted, order, direction, page, count);
        }
    }
}
