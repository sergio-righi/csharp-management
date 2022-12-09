using Application.Classes;
using Domain.Entity;
using Tool.Utilities;

namespace Application.Models.Overtime
{
    public class OvertimeFilter
    {
        public FilterViewModel<Domain.Entity.Specific.SOvertime> Filter { get; set; }
        public Pagination<Domain.Entity.Specific.SOvertime> Pagination { get; set; }

        public OvertimeFilter()
        {
            Filter = new FilterViewModel<Domain.Entity.Specific.SOvertime>();
        }
    }
}