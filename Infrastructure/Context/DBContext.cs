using Microsoft.EntityFrameworkCore;
using Domain.Entity;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;
using Infrastructure.Mapping;
using System.Linq;

namespace Infrastructure.Context
{
    public class DBContext : DbContext
    {
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Agenda> Agenda { get; set; }
        public DbSet<Attendance> Attendances { get; set; }
        public DbSet<AttendanceTimetable> AttendanceTimetable { get; set; }
        public DbSet<Bill> Bills { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Email> Emails { get; set; }
        public DbSet<Exam> Exams { get; set; }
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<FileFolder> FileFolder { get; set; }
        public DbSet<File> Files { get; set; }
        public DbSet<Folder> Folders { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Installment> Installments { get; set; }
        public DbSet<InstallmentInfo> InstallmentInfo { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<JuridicalPerson> JuridicalPeople { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<NaturalPerson> NaturalPeople { get; set; }
        public DbSet<Overtime> Overtime { get; set; }
        public DbSet<Payroll> Payrolls { get; set; }
        public DbSet<Paycheck> Paychecks { get; set; }
        //public DbSet<PermissionGroup> PermissionGroup { get; set; }
        //public DbSet<Permission> Permissions { get; set; }
        public DbSet<Person> People { get; set; }
        public DbSet<PersonAgenda> PersonAgenda { get; set; }
        public DbSet<PersonDocument> PersonDoument { get; set; }
        public DbSet<PersonRole> PersonRole { get; set; }
        public DbSet<PersonTimetable> PersonTimetable { get; set; }
        public DbSet<PersonToken> PersonToken { get; set; }
        public DbSet<Phone> Phones { get; set; }
        public DbSet<ProductImage> ProductImage { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Rent> Rents { get; set; }
        public DbSet<RentProduct> RentProduct { get; set; }
        public DbSet<Sale> Sales { get; set; }
        public DbSet<SaleProduct> SaleProduct { get; set; }
        public DbSet<State> States { get; set; }
        public DbSet<Subgroup> Subgroups { get; set; }
        public DbSet<Timetable> Timetables { get; set; }
        public DbSet<Vacation> Vacations { get; set; }

        public DBContext(DbContextOptions<DBContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Address>(new AddressMap().Configure);
            modelBuilder.Entity<Agenda>(new AgendaMap().Configure);
            modelBuilder.Entity<Attendance>(new AttendanceMap().Configure);
            modelBuilder.Entity<AttendanceTimetable>(new AttendanceTimetableMap().Configure);
            modelBuilder.Entity<Bill>(new BillMap().Configure);
            modelBuilder.Entity<Address>(new AddressMap().Configure);
            modelBuilder.Entity<City>(new CityMap().Configure);
            modelBuilder.Entity<Country>(new CountryMap().Configure);
            modelBuilder.Entity<Email>(new EmailMap().Configure);
            modelBuilder.Entity<Exam>(new ExamMap().Configure);
            modelBuilder.Entity<Expense>(new ExpenseMap().Configure);
            modelBuilder.Entity<FileFolder>(new FileFolderMap().Configure);
            modelBuilder.Entity<File>(new FileMap().Configure);
            modelBuilder.Entity<Folder>(new FolderMap().Configure);
            modelBuilder.Entity<Group>(new GroupMap().Configure);
            modelBuilder.Entity<Item>(new ItemMap().Configure);
            modelBuilder.Entity<Installment>(new InstallmentMap().Configure);
            modelBuilder.Entity<InstallmentInfo>(new InstallmentInfoMap().Configure);
            modelBuilder.Entity<JuridicalPerson>(new JuridicalPersonMap().Configure);
            modelBuilder.Entity<Overtime>(new OvertimeMap().Configure);
            modelBuilder.Entity<Menu>(new MenuMap().Configure);
            modelBuilder.Entity<NaturalPerson>(new NaturalPersonMap().Configure);
            modelBuilder.Entity<Payroll>(new PayrollMap().Configure);
            modelBuilder.Entity<Paycheck>(new PaycheckMap().Configure);
            //modelBuilder.Entity<PermissionGroup>(new PermissionGroupMap().Configure);
            //modelBuilder.Entity<Permission>(new PermissionMap().Configure);
            modelBuilder.Entity<Person>(new PersonMap().Configure);
            modelBuilder.Entity<PersonAgenda>(new PersonAgendaMap().Configure);
            modelBuilder.Entity<PersonDocument>(new PersonDocumentMap().Configure);
            modelBuilder.Entity<PersonRole>(new PersonRoleMap().Configure);
            modelBuilder.Entity<PersonTimetable>(new PersonTimetableMap().Configure);
            modelBuilder.Entity<PersonToken>(new PersonTokenMap().Configure);
            modelBuilder.Entity<Phone>(new PhoneMap().Configure);
            modelBuilder.Entity<ProductImage>(new ProductImageMap().Configure);
            modelBuilder.Entity<Product>(new ProductMap().Configure);
            modelBuilder.Entity<Rent>(new RentMap().Configure);
            modelBuilder.Entity<RentProduct>(new RentalProductMap().Configure);
            modelBuilder.Entity<Sale>(new SaleMap().Configure);
            modelBuilder.Entity<SaleProduct>(new SaleProductMap().Configure);
            modelBuilder.Entity<State>(new StateMap().Configure);
            modelBuilder.Entity<Subgroup>(new SubgroupMap().Configure);
            modelBuilder.Entity<Timetable>(new TimetableMap().Configure);
            modelBuilder.Entity<Vacation>(new VacationMap().Configure);

            //modelBuilder.Seed();
        }
    }
}
