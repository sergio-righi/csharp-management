using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Domain.Base;
using Tool.Utilities;
using Tool.Resources;

namespace Domain.Entity
{
    public class PersonDocument : BaseEntity
    {
        [RequiredResource]
        [DisplayNameResource(nameof(Label.Document))]
        public EDocument DocumentId { get; set; }
        public int PersonId { get; set; }
        [DisplayNameResource(nameof(Label.Country))]
        public int? CountryId { get; set; }
        [DisplayNameResource(nameof(Label.State))]
        public int? StateId { get; set; }
        [StringLength(50)]
        [DisplayNameResource(nameof(Label.Number))]
        public string Number { get; set; }
        [DisplayNameResource(nameof(Label.IssueDate))]
        public DateTime? IssueDate { get; set; }
        [DisplayNameResource(nameof(Label.ExpiryDate))]
        public DateTime? ExpiryDate { get; set; }

        public virtual Person Person { get; set; }
        public virtual Country Country { get; set; }
        public virtual State State { get; set; }
    }
}
