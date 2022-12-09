using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Domain.Base;
using Tool.Utilities;

namespace Domain.Entity
{
    public class Folder : BaseEntity
    {
        public int? FolderId { get; set; }
        [StringLength(150)]
        public string Name { get; set; }

        public virtual Folder Parent { get; set; }

        public virtual ICollection<FileFolder> Files { get; set; }
        public virtual ICollection<Folder> Folders { get; set; }
    }
}
