using System;
using System.Collections.Generic;
using System.Text;
using Domain.Base;
using Tool.Utilities;
using Tool.Resources;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entity.Generic;

namespace Domain.Entity
{
    public class Exam : BaseEntity
    {
        public int? FileId { get; set; }
        [RequiredResource]
        public int PersonId { get; set; }
        [RequiredResource]
        [DisplayNameResource(nameof(Label.Exam))]
        public EExam ExamId { get; set; }
        [RequiredResource]
        [DisplayNameResource(nameof(Label.Name))]
        public string Name { get; set; }
        [MaxLengthResource(255)]
        [DisplayNameResource(nameof(Label.Description))]
        public string Description { get; set; }
        [RequiredResource]
        [DisplayNameResource(nameof(Label.Date))]
        public DateTime? Date { get; set; }

        [NotMapped]
        [RequiredResource]
        [DisplayNameResource(nameof(Label.Employee))]
        public string NotName { get; set; }
        [NotMapped]
        [DisplayNameResource(nameof(Label.File))]
        public GFormFile NotFile { get; set; }

        public virtual File File { get; set; }
        public virtual Person Person { get; set; }
    }
}
