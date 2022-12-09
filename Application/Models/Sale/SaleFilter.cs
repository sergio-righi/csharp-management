using Application.Classes;
using Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Tool.Utilities;

namespace Application.Models.Sale
{
    public class SaleFilter
    {
        public FilterViewModel<Domain.Entity.Sale> Filter { get; set; }
        public Pagination<Domain.Entity.Sale> Pagination { get; set; }

        public SaleFilter()
        {
            Filter = new FilterViewModel<Domain.Entity.Sale>();
        }
    }
}