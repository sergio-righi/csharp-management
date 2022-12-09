using Application.Classes;
using Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Tool.Utilities;

namespace Application.Models.Product
{
    public class ProductFilter
    {
        public FilterViewModel<Domain.Entity.Product> Filter { get; set; }
        public Pagination<Domain.Entity.Product> Pagination { get; set; }

        public ProductFilter()
        {
            Filter = new FilterViewModel<Domain.Entity.Product>();
        }
    }
}