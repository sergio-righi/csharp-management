using Application.Classes;
using Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Tool.Utilities;

namespace Application.Models.JuridicalPerson
{
    public class JuridicalPersonFilter
    {
        public FilterViewModel<Domain.Entity.JuridicalPerson> Filter { get; set; }
        public Pagination<Domain.Entity.JuridicalPerson> Pagination { get; set; }

        public JuridicalPersonFilter()
        {
            Filter = new FilterViewModel<Domain.Entity.JuridicalPerson>();
        }
    }
}