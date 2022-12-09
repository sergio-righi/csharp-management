using Application.Classes;
using Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Tool.Utilities;

namespace Application.Models.Rent
{
    public class RentFilter
    {
        public FilterViewModel<Domain.Entity.Rent> Filter { get; set; }
        public Pagination<Domain.Entity.Rent> Pagination { get; set; }

        //contructor method
        public RentFilter()
        {
            //initialization
            Filter = new FilterViewModel<Domain.Entity.Rent>();
        }
    }
}