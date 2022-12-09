using Application.Models;
using Application.Classes;
using Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Tool.Utilities;

namespace Application.Models.NaturalPerson
{
    public class NaturalPersonFilter
    {
        public FilterViewModel<Domain.Entity.NaturalPerson> Filter { get; set; }
        public Pagination<Domain.Entity.NaturalPerson> Pagination { get; set; }

        public NaturalPersonFilter()
        {
            Filter = new FilterViewModel<Domain.Entity.NaturalPerson>();
        }
    }
}