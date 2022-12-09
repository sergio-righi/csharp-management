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
    public class Item : BaseEntity
    {
        public int? ItemId { get; set; }
        [RequiredResource]
        [StringLength(150)]
        [DisplayNameResource(nameof(Label.Name))]
        public string Name { get; set; }
        [StringLength(250)]
        [DisplayNameResource(nameof(Label.Description))]
        public string Description { get; set; }
        [DisplayNameResource(nameof(Label.Activated))]
        public bool IsActivated { get; set; }
        public bool IsDeleted { get; set; }

        [NotMapped]
        [DisplayNameResource(nameof(Label.Category))]
        public string NotName { get; set; }

        public virtual Item Parent { get; set; }

        public virtual ICollection<Item> Items { get; set; }
        public virtual ICollection<Product> Products { get; set; }

        public string GetHierarchy()
        {
            var auxiliar = this.Parent;

            List<string> hierarchy = new List<string>();

            while (auxiliar != null)
            {
                hierarchy.Insert(0, auxiliar.Name);
                auxiliar = auxiliar.Parent;
            }

            return string.Join(" / ", hierarchy);
        }
    }
}
