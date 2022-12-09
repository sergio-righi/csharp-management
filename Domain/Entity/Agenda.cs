using Domain.Base;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Tool.Utilities;

namespace Domain.Entity
{
    public class Agenda : BaseEntity
    {
        [RequiredResource]
        public DateTime Date { get; set; }
        public int? CustomerId { get; set; }
        public int? AddressId { get; set; }
        [StringLength(255)]
        public string Description { get; set; }
        [RequiredResource]
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }

        public virtual Person Customer { get; set; }
        public virtual Address Address { get; set; }

        public virtual ICollection<PersonAgenda> PersonAgenda { get; set; }
    }
}
