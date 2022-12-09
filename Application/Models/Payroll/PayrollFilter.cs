using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tool.Utilities;

namespace Application.Models.Payroll
{
    public class PayrollFilter
    {
        public FilterViewModel<Domain.Entity.Payroll> Filter { get; set; }
        public Pagination<Domain.Entity.Payroll> Pagination { get; set; }

        public PayrollFilter()
        {
            Filter = new FilterViewModel<Domain.Entity.Payroll>();
        }
    }
}
