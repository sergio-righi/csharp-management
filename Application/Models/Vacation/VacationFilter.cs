using Application.Classes;
using Domain.Entity;
using Tool.Utilities;

namespace Application.Models.Vacation
{
    public class VacationFilter
    {
        public FilterViewModel<Domain.Entity.Specific.SVacation> Filter { get; set; }
        public Pagination<Domain.Entity.Specific.SVacation> Pagination { get; set; }

        public VacationFilter()
        {
            Filter = new FilterViewModel<Domain.Entity.Specific.SVacation>();
        }
    }
}