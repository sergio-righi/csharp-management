using Application.Classes;
using Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Tool.Utilities;

namespace Application.Models.Subgroup
{
    public class SubgroupFilter
    {
        public FilterViewModel<Domain.Entity.Subgroup> Filter { get; set; }
        public Pagination<Domain.Entity.Subgroup> Pagination { get; set; }

        public SubgroupFilter()
        {
            Filter = new FilterViewModel<Domain.Entity.Subgroup>();
        }
    }
}