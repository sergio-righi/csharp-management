using System;
using System.Collections.Generic;
using System.Text;
using Domain.Base;

namespace Domain.Entity
{
    public class QuizGroup : BaseGroup
    {
        public int QuizId { get; set; }
        public bool IsActivated { get; set; }

        public virtual Quiz Quiz { get; set; }
    }
}
