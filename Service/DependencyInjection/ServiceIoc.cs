using Domain.Entity;
using Domain.Interface;
using Infrastructure.Repository.Interface;
using Microsoft.Extensions.DependencyInjection;
using Service.Service;
using Service.Service.Base;
using Service.Service.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.DependencyInjection
{
    public static class ServiceIoC
    {
        public static void ApplicationServicesIoC(this IServiceCollection services)
        {
            services.AddScoped(typeof(IService<>), typeof(BaseService<>));

            services.AddScoped<IAddressService, AddressService>();
            services.AddScoped<IAgendaService, AgendaService>();
            services.AddScoped<IAttendanceService, AttendanceService>();
            services.AddScoped<IAttendanceTimetableService, AttendanceTimetableService>();
            services.AddScoped<IBillService, BillService>();
            services.AddScoped<ICityService, CityService>();
            services.AddScoped<ICountryService, CountryService>();
            services.AddScoped<IEmailService, MailService>();
            services.AddScoped<IExamService, ExamService>();
            services.AddScoped<IExpenseService, ExpenseService>();
            services.AddScoped<IFileFolderService, FileFolderService>();
            services.AddScoped<IFileService, FileService>();
            services.AddScoped<IFolderService, FolderService>();
            services.AddScoped<IItemService, ItemService>();
            services.AddScoped<IInstallmentService, InstallmentService>();
            services.AddScoped<IInstallmentInfoService, InstallmentInfoService>();
            services.AddScoped<IOvertimeService, OvertimeService>();
            services.AddScoped<IGroupService, GroupService>();
            services.AddScoped<IMenuService, MenuService>();
            services.AddScoped<IPayrollService, PayrollService>();
            services.AddScoped<IPaycheckService, PaycheckService>();
            services.AddScoped<IPermissionGroupService, PermissionGroupService>();
            services.AddScoped<IPermissionService, PermissionService>();
            services.AddScoped<IPersonService, PersonService>();
            services.AddScoped<IPersonAgendaService, PersonAgendaService>();
            services.AddScoped<IPersonDocumentService, PersonDocumentService>();
            services.AddScoped<IPersonRoleService, PersonRoleService>();
            services.AddScoped<IPersonTimetableService, PersonTimetableService>();
            services.AddScoped<IPersonTokenService, PersonTokenService>();
            services.AddScoped<IJuridicalPersonService, JuridicalPersonService>();
            services.AddScoped<INaturalPersonService, NaturalPersonService>();
            services.AddScoped<IPhoneService, PhoneService>();
            services.AddScoped<IProductImageService, ProductImageService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IRentService, RentService>();
            services.AddScoped<IRentProductService, RentProductService>();
            services.AddScoped<ISaleService, SaleService>();
            services.AddScoped<ISaleProductService, SaleProductService>();
            services.AddScoped<IStateService, StateService>();
            services.AddScoped<ISubgroupService, SubgroupService>();
            services.AddScoped<ITimetableService, TimetableService>();
            services.AddScoped<IVacationService, VacationService>();
        }
    }
}
