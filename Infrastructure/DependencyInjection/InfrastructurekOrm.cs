using Domain.Entity;
using Domain.Interface;
using Infrastructure.Context;
using Infrastructure.DependencyInjection.Base;
using Infrastructure.Repository;
using Infrastructure.Repository.Base;
using Infrastructure.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.DependencyInjection
{
    public class InfrastructureOrm : BaseOrm
    {
        internal override IServiceCollection AddOrm(IServiceCollection services, IConfiguration configuration = null)
        {
            //IConfiguration dbConnectionSettings = ResolveConfiguration.GetConnectionSettings(configuration);

            services.AddDbContext<DBContext>(options => options.UseMySql(connectionString: DatabaseConnection.ConnectionString));

            services.AddScoped(typeof(IRepository<>), typeof(BaseRepository<>));

            services.AddScoped<IAddressRepository, AddressRepository>();
            services.AddScoped<IAgendaRepository, AgendaRepository>();
            services.AddScoped<IAttendanceRepository, AttendanceRepository>();
            services.AddScoped<IAttendanceTimetableRepository, AttendanceTimetableRepository>();
            services.AddScoped<IBillRepository, BillRepository>();
            services.AddScoped<ICityRepository, CityRepository>();
            services.AddScoped<ICountryRepository, CountryRepository>();
            services.AddScoped<IEmailRepository, EmailRepository>();
            services.AddScoped<IExamRepository, ExamRepository>();
            services.AddScoped<IExpenseRepository, ExpenseRepository>();
            services.AddScoped<IFileFolderRepository, FileFolderRepository>();
            services.AddScoped<IFileRepository, FileRepository>();
            services.AddScoped<IFolderRepository, FolderRepository>();
            services.AddScoped<IGroupRepository, GroupRepository>();
            services.AddScoped<IItemRepository, ItemRepository>();
            services.AddScoped<IInstallmentRepository, InstallmentRepository>();
            services.AddScoped<IInstallmentInfoRepository, InstallmentInfoRepository>();
            services.AddScoped<IOvertimeRepository, OvertimeRepository>();
            services.AddScoped<IMenuRepository, MenuRepository>();
            services.AddScoped<IPayrollRepository, PayrollRepository>();
            services.AddScoped<IPaycheckRepository, PaycheckRepository>();
            services.AddScoped<IPermissionGroupRepository, PermissionGroupRepository>();
            services.AddScoped<IPermissionRepository, PermissionRepository>();
            services.AddScoped<IPersonRepository, PersonRepository>();
            services.AddScoped<IPersonAgendaRepository, PersonAgendaRepository>();
            services.AddScoped<IPersonDocumentRepository, PersonDocumentRepository>();
            services.AddScoped<IPersonRoleRepository, PersonRoleRepository>();
            services.AddScoped<IPersonTimetableRepository, PersonTimetableRepository>();
            services.AddScoped<IPersonTokenRepository, PersonTokenRepository>();
            services.AddScoped<IJuridicalPersonRepository, JuridicalPersonRepository>();
            services.AddScoped<INaturalPersonRepository, NaturalPersonRepository>();
            services.AddScoped<IPhoneRepository, PhoneRepository>();
            services.AddScoped<IProductImageRepository, ProductImageRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IRentProductRepository, RentProductRepository>();
            services.AddScoped<IRentRepository, RentRepository>();
            services.AddScoped<ISaleRepository, SaleRepository>();
            services.AddScoped<ISaleProductRepository, SaleProductRepository>();
            services.AddScoped<IStateRepository, StateRepository>();
            services.AddScoped<ISubgroupRepository, SubgroupRepository>();
            services.AddScoped<ITimetableRepository, TimetableRepository>();
            services.AddScoped<IVacationRepository, VacationRepository>();

            return services;
        }
    }
}
