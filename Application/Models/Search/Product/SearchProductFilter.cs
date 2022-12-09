using Application.Classes;
using Domain.Entity;
using Domain.Entity.Specific;
using System.Collections.Generic;
using Tool.Utilities;

namespace Application.Models.Search.Product
{
    public class SearchProductFilter
    {
        public ICollection<SProduct> Products { get; set; }
        public Pagination<SProduct> Pagination { get; set; }
        public FilterViewModel<SProduct> Filter { get; set; }
        public SearchProductConfig Configuration { get; set; }

        public SearchProductFilter()
        {
            Filter = new FilterViewModel<SProduct>()
            {
                Count = 40
            };
        }
    }
}