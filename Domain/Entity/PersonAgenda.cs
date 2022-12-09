using Domain.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entity
{
    public class PersonAgenda : BaseEntity
    {
        public int AgendaId { get; set; }
        public int PersonId { get; set; }

        public virtual Agenda Agenda { get; set; }
        public virtual Person Person { get; set; }
    }
}
