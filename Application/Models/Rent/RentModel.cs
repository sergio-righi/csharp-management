using Application.Classes;
using Domain.Entity;
using Domain.Entity.Generic;
using Domain.Entity.Specific;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Application.Models.Rent
{
    public class RentModel
    {
        public Domain.Entity.Rent Rent { get; set; }
        public ICollection<SProduct> Products { get; set; }
        public ICollection<GCustomInformation> Addresses { get; set; }

        public RentModel()
        {
            Rent = new Domain.Entity.Rent();
        }
    }
}