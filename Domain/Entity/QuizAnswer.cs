using System;
using System.Collections.Generic;
using System.Text;
using Domain.Base;

namespace Domain.Entity
{
    public class QuizAnswer : BaseEntity
    {
        public int PersonId { get; set; }
        public int QuestionId { get; set; }
        public string Answer { get; set; }

        public virtual Person Person { get; set; }
        public virtual QuizQuestion Question { get; set; }
    }
}
