using Domain.Entity;
using Domain.Interface;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Tool.Utilities;

namespace Service.Service.Interface
{
    public interface IFileService : IService<File>
    {
        Task<File> Find(string key, bool? activated, bool? deleted);
        Task<File> FindPhoto(int personId);

        Task<ICollection<string>> ListExtension(int? personId, bool? activated, bool? deleted);
        Task<ICollection<File>> List(string name, int? category, int? personId, int? folderId, bool? activated, bool? deleted, Expression<Func<File, object>> order, EDirection direction, int count);
        Task<Pagination<File>> List(string name, int? category, int? personId, int? folderId, bool? activated, bool? deleted, Expression<Func<File, object>> order, EDirection direction, int page, int count);
    }
}
