using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Domain.Base;
using Tool.Utilities;

namespace Domain.Entity
{
    public class City : BaseEntity
    {
        public int? StateId { get; set; }
        public int? CountryId { get; set; }
        [StringLength(150)]
        public string Name { get; set; }
        public string PhoneCode { get; set; }
        public bool IsActivated { get; set; }

        public virtual Country Country { get; set; }
        public virtual State State { get; set; }

        public virtual ICollection<Address> Addresses { get; set; }
    }
}
