using Application.Classes;
using Domain.Entity;
using Domain.Entity.Generic;
using Domain.Entity.Specific;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Application.Models.Sale
{
    public class SaleModel
    {
        public Domain.Entity.Sale Sale { get; set; }
        public ICollection<GCustomInformation> Addresses { get; set; }
        public ICollection<SProduct> Products { get; set; }

        public SaleModel()
        {
            Sale = new Domain.Entity.Sale();
        }
    }
}