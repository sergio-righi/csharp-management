using System;
using System.Collections.Generic;
using System.Text;
using Domain.Base;
using Tool.Utilities;

namespace Domain.Entity
{
    public class QuizQuestion : BaseEntity
    {
        public EQuizQuestion TypeId { get; set; }
        public string Statement { get; set; }
        public bool IsRequired { get; set; }
    }
}
