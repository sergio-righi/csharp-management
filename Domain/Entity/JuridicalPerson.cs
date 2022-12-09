using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Domain.Base;
using Tool.Utilities;
using Tool.Resources;

namespace Domain.Entity
{
    public class JuridicalPerson : BaseEntity
    {
        [DisplayNameResource(nameof(Label.Activity))]
        public EBusinessActivity ActivityId { get; set; }
        [DisplayNameResource(nameof(Label.Type))]
        public EBusinessType TypeId { get; set; }
        [DisplayNameResource(nameof(Label.Area))]
        public EBusinessArea AreaId { get; set; }
        [RequiredResource]
        [StringLength(150)]
        [DisplayNameResource(nameof(Label.FantasyName))]
        public string FantasyName { get; set; }
        [RequiredResource]
        [StringLength(150)]
        [DisplayNameResource(nameof(Label.CompanyName))]
        public string CompanyName { get; set; }
        [StringLength(250)]
        [DisplayNameResource(nameof(Label.Description))]
        public string Description { get; set; }
        [RequiredResource]
        [StringLength(14)]
        [DisplayNameResource(nameof(Label.CNPJ))]
        public string CNPJ { get; set; }

        public virtual Person Person { get; set; }
    }
}
