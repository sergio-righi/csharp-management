using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Domain.Base;
using Tool.Utilities;

namespace Domain.Entity
{
    public class File : BaseEntity
    {
        [StringLength(150)]
        public string Name { get; set; }
        [StringLength(250)]
        public string Description { get; set; }
        [StringLength(100)]
        public string GeneratedName { get; set; }
        [StringLength(5)]
        public string Extension { get; set; }
        [StringLength(100)]
        public string Key { get; set; }
        public long Size { get; set; }
        public EExtension ExtensionId { get; set; }
        public bool IsActivated { get; set; }
        public bool IsDeleted { get; set; }

        [NotMapped]
        public string NotGeneratedName
        {
            get
            {
                return $"{GeneratedName}{Extension}";
            }
        }

        [NotMapped]
        public string NotName
        {
            get
            {
                return $"{Name}{Extension}";
            }
        }

        public virtual Bill Bill { get; set; }
        public virtual Bill Receipt { get; set; }
        public virtual Person Person { get; set; }
        public virtual Attendance Attendance { get; set; }

        public virtual ICollection<Expense> Expenses { get; set; }
        public virtual ICollection<FileFolder> Folders { get; set; }
        public virtual ICollection<Paycheck> Paychecks { get; set; }
        public virtual ICollection<ProductImage> Images { get; set; }
        public virtual ICollection<Vacation> Vacations { get; set; }
        public virtual ICollection<Exam> Exams { get; set; }
        public virtual ICollection<Overtime> Overtimes { get; set; }
    }
}
