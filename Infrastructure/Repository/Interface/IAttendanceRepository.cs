using Domain.Entity;
using Domain.Entity.Specific;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Tool.Utilities;

namespace Infrastructure.Repository.Interface
{
    public interface IAttendanceRepository : IDomainRepository<Attendance>
    {
        Task<SAttendancePerson> Find(int id, DateTime date);
        Task<ICollection<SAttendancePerson>> List(DateTime date);
        Task<ICollection<DateTime>> List(int month, int year);
        Task Manage(ICollection<Attendance> added, ICollection<Attendance> modified);
    }
}
