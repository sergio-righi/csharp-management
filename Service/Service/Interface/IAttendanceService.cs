using Domain.Entity;
using Domain.Entity.Specific;
using Domain.Interface;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Tool.Utilities;

namespace Service.Service.Interface
{
    public interface IAttendanceService : IService<Attendance>
    {
        Task<SAttendancePerson> Find(int id, DateTime date);
        Task<ICollection<SAttendancePerson>> List(DateTime date);
        Task<ICollection<DateTime>> List(int month, int year);
        Task Manage(ICollection<Attendance> added, ICollection<Attendance> modified);
    }
}
