using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tool.Utilities;

namespace Application.Models.Paycheck
{
    public class PaycheckFilter
    {
        public FilterViewModel<Domain.Entity.Specific.SPaycheckPerson> Filter { get; set; }
        public Pagination<Domain.Entity.Specific.SPaycheckPerson> Pagination { get; set; }

        public PaycheckFilter()
        {
            Filter = new FilterViewModel<Domain.Entity.Specific.SPaycheckPerson>();
        }
    }
}
