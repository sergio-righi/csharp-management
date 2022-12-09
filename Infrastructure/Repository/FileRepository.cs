using Domain.Entity;
using Infrastructure.Context;
using Infrastructure.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Tool.Utilities;
using System.Linq.Expressions;
using Tool.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository
{
    public class FileRepository : DomainRepository<File>, IFileRepository
    {
        public FileRepository(DBContext dbContext) : base(dbContext)
        {
        }

        public async Task<File> Find(string key, bool? activated, bool? deleted)
        {
            var query = (from F in DbContext.Files
                         where F.Key.Equals(key) && F.IsActivated == (activated ?? F.IsActivated) && F.IsDeleted == (deleted ?? F.IsDeleted)
                         select F)
                         .FirstOrDefault();

            return await Task.FromResult(query);
        }

        public async Task<File> FindPhoto(int personId)
        {
            var query = (from F in DbContext.Files
                         join P in DbContext.People on F.Id equals P.PhotoId
                         where P.Id == personId
                         select F)
                         .FirstOrDefault();

            return await Task.FromResult(query);
        }

        public async Task<ICollection<string>> ListExtension(int? personId, bool? activated, bool? deleted)
        {
            var query = (from F in DbContext.Files
                         where
                          F.CreatedBy == (personId ?? F.CreatedBy) && F.IsActivated == (activated ?? F.IsActivated) && F.IsDeleted == (deleted ?? F.IsDeleted)
                         orderby F.Extension
                         select F.Extension)
                         .Distinct()
                         .AsNoTracking()
                         .ToList();

            return await Task.FromResult(query);
        }

        public async Task<ICollection<File>> List(string name, int? category, int? personId, int? folderId, bool? activated, bool? deleted, Expression<Func<File, object>> order, EDirection direction, int count)
        {
            var query = (from F in DbContext.Files
                         join FF in DbContext.FileFolder on F.Id equals FF.FileId into FF_Join
                         from FF in FF_Join.DefaultIfEmpty()
                         where
                          (!string.IsNullOrWhiteSpace(name) ? F.Name.Contains(name) : true) && (int)F.ExtensionId == (category ?? (int)F.ExtensionId) && F.CreatedBy == (personId ?? F.CreatedBy) && ((folderId == null && FF.Id == null) || (folderId != null && FF.FolderId == folderId)) && F.IsActivated == (activated ?? F.IsActivated) && F.IsDeleted == (deleted ?? F.IsDeleted)
                         select F)
                         .OrderBy(order, direction)
                         .Take(count)
                         .AsNoTracking()
                         .ToList();

            return await Task.FromResult(query);
        }

        public async Task<Pagination<File>> List(string name, int? category, int? personId, int? folderId, bool? activated, bool? deleted, Expression<Func<File, object>> order, EDirection direction, int page, int count)
        {
            var query = (from F in DbContext.Files
                         join FF in DbContext.FileFolder on F.Id equals FF.FileId into FF_Join
                         from FF in FF_Join.DefaultIfEmpty()
                         where
                          (!string.IsNullOrWhiteSpace(name) ? F.Name.Contains(name) : true) && (int)F.ExtensionId == (category ?? (int)F.ExtensionId) && F.CreatedBy == (personId ?? F.CreatedBy) && ((folderId == null && FF.Id == null) || (folderId != null && FF.FolderId == folderId)) && F.IsActivated == (activated ?? F.IsActivated) && F.IsDeleted == (deleted ?? F.IsDeleted)
                         select F)
                         .OrderBy(order, direction)
                         .Skip((page - 1) * count)
                         .Take(count)
                         .AsNoTracking()
                         .ToList();

            var total = (from F in DbContext.Files
                         join FF in DbContext.FileFolder on F.Id equals FF.FileId into FF_Join
                         from FF in FF_Join.DefaultIfEmpty()
                         where
                           (!string.IsNullOrWhiteSpace(name) ? F.Name.Contains(name) : true) && (int)F.ExtensionId == (category ?? (int)F.ExtensionId) && F.CreatedBy == (personId ?? F.CreatedBy) && ((folderId == null && FF.Id == null) || (folderId != null && FF.FolderId == folderId)) && F.IsActivated == (activated ?? F.IsActivated) && F.IsDeleted == (deleted ?? F.IsDeleted)
                         select F.Id)
                         .Count();

            query = await Task.FromResult(query);

            return new Pagination<File>(query, total, page, count);
        }
    }
}
