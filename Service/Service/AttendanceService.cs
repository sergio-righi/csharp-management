using Domain.Entity;
using Domain.Entity.Specific;
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
    public class AttendanceService : BaseService<Attendance>, IAttendanceService
    {
        private readonly IAttendanceRepository _repository;

        public AttendanceService(IAttendanceRepository repository) : base(repository)
        {
            _repository = repository;
        }

        public async Task<SAttendancePerson> Find(int id, DateTime date)
        {
            return await _repository.Find(id, date);
        }

        public async Task<ICollection<SAttendancePerson>> List(DateTime date)
        {
            return await _repository.List(date);
        }

        public async Task<ICollection<DateTime>> List(int month, int year)
        {
            return await _repository.List(month, year);
        }

        public async Task Manage(ICollection<Attendance> added, ICollection<Attendance> modified)
        {
            await _repository.Manage(added, modified);
        }
    }
}
