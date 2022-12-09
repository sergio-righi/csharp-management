using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Domain.Base;
using Tool.Utilities;

namespace Domain.Entity
{
    public class State : BaseEntity
    {
        public int CountryId { get; set; }
        [StringLength(150)]
        public string Name { get; set; }
        public string PhoneCode { get; set; }
        [StringLength(2)]
        public string Abbreviation { get; set; }
        public bool IsActivated { get; set; }

        [NotMapped]
        public string NotName
        {
            get
            {
                return $"{Name} ({Abbreviation})";
            }
        }

        public virtual Country Country { get; set; }

        public virtual ICollection<City> Citites { get; set; }
        public virtual ICollection<PersonDocument> Documents { get; set; }
    }
}
