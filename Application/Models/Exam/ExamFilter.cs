using Application.Classes;
using Domain.Entity;
using Tool.Utilities;

namespace Application.Models.Exam
{
    public class ExamFilter
    {
        public FilterViewModel<Domain.Entity.Generic.GCustomInformation> Filter { get; set; }
        public Pagination<Domain.Entity.Generic.GCustomInformation> Pagination { get; set; }

        public ExamFilter()
        {
            Filter = new FilterViewModel<Domain.Entity.Generic.GCustomInformation>();
        }
    }
}