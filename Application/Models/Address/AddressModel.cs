using Domain.Entity;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Models.Address
{
    public class AddressModel
    {
        public Domain.Entity.Address Address { get; set; }
        public ICollection<Domain.Entity.City> Citites { get; set; }
        public ICollection<Domain.Entity.State> States { get; set; }
        public ICollection<Domain.Entity.Country> Countries { get; set; }
        public ICollection<Domain.Entity.Address> Addresses { get; set; }

        public AddressModel()
        {
            Address = new Domain.Entity.Address();
        }
    }
}
