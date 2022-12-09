using Domain.Base;
using Domain.Interface;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Tool.Resources;
using Tool.Utilities;

namespace Domain.Entity
{
    public class Address : BaseEntity
    {
        [DisplayNameResource(nameof(Label.Type))]
        public EAddress TypeId { get; set; }
        [DisplayNameResource(nameof(Label.City))]
        public int CityId { get; set; }
        public int PersonId { get; set; }
        [StringLength(8)]
        [RequiredResource]
        [DisplayNameResource(nameof(Label.Zipcode))]
        public string Zipcode { get; set; }
        [RequiredResource]
        [StringLength(150)]
        [DisplayNameResource(nameof(Label.Street))]
        public string Street { get; set; }
        [RequiredResource]
        [DisplayNameResource(nameof(Label.Number))]
        public int? Number { get; set; }
        [StringLength(100)]
        [DisplayNameResource(nameof(Label.Complement))]
        public string Complement { get; set; }
        [StringLength(100)]
        [DisplayNameResource(nameof(Label.Neighborhood))]
        public string Neighborhood { get; set; }
        public bool IsDeleted { get; set; }

        [NotMapped]
        [DisplayNameResource(nameof(Label.Country))]
        public int? NotCountryId { get; set; }
        [NotMapped]
        [DisplayNameResource(nameof(Label.State))]
        public int? NotStateId { get; set; }
        [NotMapped]
        public EPerson NotPersonTypeId { get; set; }

        public virtual City City { get; set; }
        public virtual Person Person { get; set; }

        public virtual ICollection<Rent> Rents { get; set; }
        public virtual ICollection<Sale> Sales { get; set; }
        public virtual ICollection<Agenda> Agenda { get; set; }
    }
}
