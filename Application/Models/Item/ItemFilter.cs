using Application.Classes;
using Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Tool.Utilities;

namespace Application.Models.Item
{
    public class ItemFilter
    {
        public FilterViewModel<Domain.Entity.Item> Filter { get; set; }

        public Pagination<Domain.Entity.Item> Pagination { get; set; }

        public ItemFilter()
        {
            Filter = new FilterViewModel<Domain.Entity.Item>();
        }
    }
}