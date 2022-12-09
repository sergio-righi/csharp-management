using Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Tool.Utilities;

namespace Infrastructure.Repository.Interface
{
    public interface IFileRepository : IDomainRepository<File>
    {
        Task<File> Find(string key, bool? activated, bool? deleted);
        Task<File> FindPhoto(int personId);

        Task<ICollection<string>> ListExtension(int? personId, bool? activated, bool? deleted);
        Task<ICollection<File>> List(string name, int? category, int? personId, int? folderId, bool? activated, bool? deleted, Expression<Func<File, object>> order, EDirection direction, int count);
        Task<Pagination<File>> List(string name, int? category, int? personId, int? folderId, bool? activated, bool? deleted, Expression<Func<File, object>> order, EDirection direction, int page, int count);
    }
}
