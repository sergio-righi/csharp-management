using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Domain.Base;
using Tool.Utilities;
using Tool.Resources;

namespace Domain.Entity
{
    public class Group : BaseEntity
    {
        [RequiredResource]
        [StringLength(150)]
        [DisplayNameResource(nameof(Label.Name))]
        public string Name { get; set; }
        [StringLength(250)]
        [DisplayNameResource(nameof(Label.Description))]
        public string Description { get; set; }
        [DisplayNameResource(nameof(Label.Activated))]
        public bool IsActivated { get; set; }
        public virtual ICollection<Subgroup> Subgroups { get; set; }
    }
}
