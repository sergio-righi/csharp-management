using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Domain.Base;
using Domain.Interface;
using Microsoft.AspNetCore.Http;
using Tool.Utilities;
using Tool.Resources;
using Domain.Entity.Generic;

namespace Domain.Entity
{
    public class Person : BaseEntity
    {
        public int? PhotoId { get; set; }
        public EPerson TypeId { get; set; }
        [StringLength(150)]
        public string Login { get; set; }
        [StringLength(150)]
        [DataType(DataType.Password)]
        [DisplayNameResource(nameof(Label.Password))]
        public string Password { get; set; }
        [DisplayNameResource(nameof(Label.Activated))]
        public bool IsActivated { get; set; }
        public bool IsDeleted { get; set; }

        [NotMapped]
        [DataType(DataType.Password)]
        [CompareResource(nameof(Password))]
        [DisplayNameResource(nameof(Label.PasswordConfirm))]
        public string NotPassword { get; set; }
        [NotMapped]
        public GFormFile NotPhoto { get; set; }

        public virtual File Photo { get; set; }
        public virtual JuridicalPerson JuridicalPerson { get; set; }
        public virtual NaturalPerson NaturalPerson { get; set; }

        public virtual ICollection<Address> Addresses { get; set; }
        public virtual ICollection<Email> Emails { get; set; }
        public virtual ICollection<Phone> Phones { get; set; }
        public virtual ICollection<Rent> EmployeeRents { get; set; }
        public virtual ICollection<Rent> ClientRents { get; set; }
        public virtual ICollection<PersonRole> Roles { get; set; }
        public virtual ICollection<PermissionGroup> PermissionGroups { get; set; }
        public virtual ICollection<Attendance> Attendances { get; set; }
        public virtual ICollection<Bill> Bills { get; set; }
        public virtual ICollection<Exam> Exams { get; set; }
        public virtual ICollection<Overtime> Overtimes { get; set; }
        public virtual ICollection<Sale> Sales { get; set; }
        public virtual ICollection<Sale> Purchases { get; set; }
        public virtual ICollection<Sale> Providers { get; set; }
        public virtual ICollection<Vacation> Vacations { get; set; }
        public virtual ICollection<PersonDocument> Documents { get; set; }
        public virtual ICollection<PersonToken> Tokes { get; set; }
        public virtual ICollection<Paycheck> Paychecks { get; set; }
        public virtual ICollection<Expense> Expenses { get; set; }
        public virtual ICollection<Agenda> Agenda { get; set; }
        public virtual ICollection<PersonAgenda> PersonAgenda { get; set; }
        public virtual ICollection<PersonTimetable> PersonTimetable { get; set; }
    }
}
