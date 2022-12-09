using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Domain.Base;
using Tool.Utilities;
using Tool.Resources;

namespace Domain.Entity
{
    public class Phone : BaseEntity
    {
        [DisplayNameResource(nameof(Label.Type))]
        public EPhone TypeId { get; set; }
        public int PersonId { get; set; }
        [RequiredResource]
        [StringLength(14)]
        [DisplayNameResource(nameof(Label.Phone))]
        public string Number { get; set; }
        public bool IsDeleted { get; set; }

        [NotMapped]
        public EPerson NotPersonTypeId { get; set; }

        public virtual Person Person { get; set; }
    }
}
