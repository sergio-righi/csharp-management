using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Domain.Base;
using Tool.Utilities;

namespace Domain.Entity
{
    public class Country : BaseEntity
    {
        [StringLength(150)]
        public string Name { get; set; }
        public string PhoneCode { get; set; }
        public string Code { get; set; }
        [StringLength(3)]
        public string Abbreviation { get; set; }
        public bool IsActivated { get; set; }

        public virtual ICollection<City> Citites { get; set; }
        public virtual ICollection<State> States { get; set; }
        public virtual ICollection<PersonDocument> Documents { get; set; }
    }
}
