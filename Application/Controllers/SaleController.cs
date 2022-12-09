using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Application.Classes;
using Application.Controllers.Base;
using Application.Extensions;
using Application.Models;
using Application.Models.Sale;
using Domain.Entity;
using Microsoft.AspNetCore.Mvc;
using Service.Service.Interface;
using Tool.Configurations;
using Tool.Extensions;
using Tool.Resources;
using Tool.Utilities;

namespace Application.Controllers
{
    [Route("venda/")]
    public partial class SaleController : Base.ProductController
    {
        private readonly ISaleService SaleService;
        private readonly IPersonService PersonService;
        private readonly IAddressService AddressService;
        private readonly IInstallmentService InstallmentService;
        private readonly ISaleProductService SaleProductService;

        public SaleController(ISaleService saleService,
                              IPersonService personService,
                              IAddressService addressService,
                              IInstallmentService installmentService,
                              ISaleProductService saleProductService) : base(saleProductService)
        {
            SaleService = saleService;
            PersonService = personService;
            AddressService = addressService;
            InstallmentService = installmentService;
            SaleProductService = saleProductService;
        }

        [Route("")]
        public async virtual Task<IActionResult> Index(FilterViewModel<Sale> filter)
        {
            if (hasSession())
            {
                var saleFilter = new SaleFilter();

                saleFilter.Pagination = await SaleService.List(personId: filter.ReferenceId,
                                                               situationId: (ESale)(filter.SituationId ?? 0),
                                                               order: GetOrder(filter.Sort),
                                                               direction: filter.Direction,
                                                               page: filter.Page,
                                                               count: filter.Count);

                saleFilter.Filter = filter;

                return View(saleFilter);
            }

            return Logout(Message.SessionExpired);
        }

        [Route(RouteManageId)]
        public async virtual Task<IActionResult> Manage(int id)
        {
            if (hasSession())
            {
                var model = new SaleModel();

                if (id.IsPositive())
                {
                    model.Sale = await SaleService.Find(id);

                    if (model == null)
                    {
                        return NotFound();
                    }

                    SetManageInformation(model);

                    model.Products = await SaleProductService.SList(id);
                }

                return View(model);
            }

            return Logout(Message.SessionExpired);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        [Route(RouteManageId)]
        public async virtual Task<IActionResult> Manage(SaleModel model)
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
                            if (model.Sale.InstallmentId.IsPositive())
                            {
                                await InstallmentService.Manage(installment);
                            }
                            else
                            {
                                model.Sale.InstallmentId = (await InstallmentService.Insert(installment))?.Id;
                            }

                            model.Sale.InstallmentCount = installment.Dates.Count;
                        }
                        else
                        {
                            model.Sale.InstallmentCount = await InstallmentService.Count(model.Sale.InstallmentId);
                        }

                        if (model.Sale.Id.IsZero())
                        {
                            model.Sale.SellerId = GetCurrentUser();
                            model.Sale.CreatedBy = GetCurrentUser();

                            model.Sale = await SaleService.Insert(model.Sale);
                        }
                        else
                        {
                            model.Sale.UpdatedBy = GetCurrentUser();

                            await SaleService.Update(model.Sale);

                            await base.ManageSale(model.Sale.Id, model.Products);
                        }

                        return RedirectToAction(nameof(Manage), new { id = model.Sale.Id }).WithSuccess(Message.SuccessOnSave);
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

        [Route(RouteDeleteId)]
        public async virtual Task<IActionResult> Delete(int id)
        {
            if (hasSession())
            {
                if (id.IsPositive())
                {
                    var model = await SaleService.Find(id);

                    try
                    {
                        if (model == null)
                        {
                            return RedirectToAction(nameof(Index)).WithWarning(Message.NotFound);
                        }

                        //await _saleService.Delete(model);

                        return RedirectToAction(nameof(Index)).WithSuccess(Message.SuccessOnDelete);
                    }
                    catch
                    {
                        return View(nameof(Manage), model).WithError(Message.ErrorOnDelete);
                    }
                }

                return BadRequest();
            }

            return Logout(Message.SessionExpired);
        }

        [HttpPost]
        [Route("sincronizar/{id:int}")]
        public async virtual Task<IActionResult> Sync(int id)
        {
            if (hasSession())
            {
                return PartialView("~/Views/Sale/_List.cshtml", (await SaleProductService.SList(id)).ToList());
            }

            return Logout(Message.Logout);
        }

        private async void SetManageInformation(SaleModel model)
        {
            model.Addresses = await AddressService.GList(model.Sale.CustomerId, false);

            model.Sale.NotCustomerName = (await PersonService.GFind(model.Sale.CustomerId))?.Name;
        }

        private static Expression<Func<Sale, object>> GetOrder(int sort)
        {
            switch (sort)
            {
                case 1:
                    return x => x.Customer.NaturalPerson.GivenName;
                case 2:
                    return x => x.Date;
            }

            return null;
        }
    }
}