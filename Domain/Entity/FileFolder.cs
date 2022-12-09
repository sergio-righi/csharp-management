using System;
using System.Collections.Generic;
using System.Text;
using Domain.Base;
using Tool.Utilities;

namespace Domain.Entity
{
    public class FileFolder : BaseEntity
    {
        public int FileId { get; set; }
        public int FolderId { get; set; }

        public virtual File File { get; set; }
        public virtual Folder Folder { get; set; }
    }
}
