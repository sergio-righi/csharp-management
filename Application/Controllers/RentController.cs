using Application.Classes;
using Application.Controllers.Base;
using Application.Extensions;
using Application.Models;
using Domain.Entity;
using Microsoft.AspNetCore.Mvc;
using Service.Service.Interface;
using System;
using System.Linq;
using System.Threading.Tasks;
using Tool.Resources;
using Tool.Extensions;
using Tool.Utilities;
using System.Linq.Expressions;
using Application.Models.Rent;
using Service.Service;
using System.Collections.Generic;
using Domain.Entity.Specific;
using Microsoft.EntityFrameworkCore.Update.Internal;
using Tool.Configurations;

namespace Application.Controllers
{
    [Route("aluguel/")]
    public partial class RentController : Base.ProductController
    {
        private readonly IItemService ItemService;
        private readonly IRentService RentService;
        private readonly IPersonService PersonService;
        private readonly IAddressService AddressService;
        private readonly IInstallmentService InstallmentService;
        private readonly IRentProductService RentProductService;

        public RentController(IItemService itemService,
                              IRentService rentService,
                              IPersonService personService,
                              IAddressService addressService,
                              IInstallmentService installmentService,
                              IRentProductService rentProductService) : base(rentProductService)
        {
            ItemService = itemService;
            RentService = rentService;
            PersonService = personService;
            AddressService = addressService;
            InstallmentService = installmentService;
            RentProductService = rentProductService;
        }

        [Route("")]
        public async virtual Task<IActionResult> Index(FilterViewModel<Rent> filter)
        {
            if (hasSession())
            {
                var rentFilter = new RentFilter();

                if (filter.StartDate != null || filter.EndDate != null || filter.SituationId != null)
                {
                    rentFilter.Pagination = await RentService.GetQueryablePagination(filter: x => x.StartDate >= (filter.StartDate ?? x.StartDate) && x.EndDate <= (filter.EndDate ?? x.EndDate) && (int)x.SituationId == (filter.SituationId ?? (int)x.SituationId),
                                                                                     order: GetOrder(filter.Sort),
                                                                                     direction: filter.Direction,
                                                                                     page: filter.Page,
                                                                                     count: filter.Count);
                }
                else
                {
                    rentFilter.Pagination = await RentService.GetQueryablePagination(order: GetOrder(filter.Sort), direction: filter.Direction, page: filter.Page, count: filter.Count);
                }

                rentFilter.Filter = filter;

                return View(rentFilter);
            }

            return Logout(Message.SessionExpired);
        }

        [Route(RouteManageId)]
        public async virtual Task<IActionResult> Manage(int id)
        {
            if (hasSession())
            {
                var model = new RentModel();

                if (id.IsPositive())
                {
                    model.Rent = await RentService.Find(id);

                    if (model == null)
                    {
                        return NotFound();
                    }

                    SetManageInformation(model);

                    model.Products = await RentProductService.SList(id);
                }

                return View(model);
            }

            return Logout(Message.SessionExpired);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        [Route(RouteManageId)]
        public async virtual Task<IActionResult> Manage(RentModel model)
        {
            if (hasSession())
            {
                SetManageInformation(model);

                if (ModelState.IsValid)
                {
                    try
                    {
                        var installment = HttpContext.Session.Get<Installment>(AppConfig.Session.Installment);

                        if (installment != null)
                        {
                            if (model.Rent.InstallmentId.IsPositive())
                            {
                                await InstallmentService.Manage(installment);
                            }
                            else
                            {
                                model.Rent.InstallmentId = (await InstallmentService.Insert(installment))?.Id;
                            }

                            model.Rent.InstallmentCount = installment.Dates.Count;
                        }
                        else
                        {
                            model.Rent.InstallmentCount = await InstallmentService.Count(model.Rent.InstallmentId);
                        }

                        if (model.Rent.Id.IsZero())
                        {
                            model.Rent.EmployeeId = GetCurrentUser();
                            model.Rent.CreatedBy = GetCurrentUser();

                            model.Rent = await RentService.Insert(model.Rent);
                        }
                        else
                        {
                            model.Rent.UpdatedBy = GetCurrentUser();

                            await RentService.Update(model.Rent);

                            await base.ManageRent(model.Rent.Id, model.Products);
                        }

                        return RedirectToAction(nameof(Manage), new { id = model.Rent.Id }).WithSuccess(Message.SuccessOnSave);
                    }
                    catch
                    {
                        return View(model).WithError(Message.InternalServerError);
                    }
                }
                else
                {
                    return View(model).WithWarning(Message.FillAllRequiredField);
                }
            }

            return Logout(Message.SessionExpired);
        }

        [HttpPost]
        [Route("sincronizar/{id:int}")]
        public async virtual Task<IActionResult> Sync(int id)
        {
            if (hasSession())
            {
                return PartialView("~/Views/Rent/_List.cshtml", (await RentProductService.SList(id)).ToList());
            }

            return Logout(Message.Logout);
        }

        private async void SetManageInformation(RentModel model)
        {
            model.Addresses = await AddressService.GList(model.Rent.CustomerId, false);

            model.Rent.NotCustomerName = (await PersonService.GFind(model.Rent.CustomerId))?.Name;
        }

        private static Expression<Func<Rent, object>> GetOrder(int sort)
        {
            switch (sort)
            {
                case 1:
                    return x => x.StartDate;
                case 2:
                    return x => x.EndDate;
                case 3:
                    return x => x.SituationId;
            }

            return null;
        }
    }
}
