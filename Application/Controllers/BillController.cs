using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Application.Classes;
using Application.Classes.Attribute;
using Application.Controllers.Base;
using Application.Extensions;
using Application.Models;
using Application.Models.Bill;
using Domain.Entity;
using Microsoft.AspNetCore.Mvc;
using Service.Service.Interface;
using Tool.Configurations;
using Tool.Extensions;
using Tool.Resources;
using Tool.Utilities;

namespace Application.Controllers
{
    [Route("conta/")]
    public partial class BillController : BaseController
    {
        private readonly IBillService BillService;
        private readonly IFileService FileService;
        private readonly IPersonService PersonService;
        private readonly IInstallmentService InstallmentService;

        public BillController(IBillService billService,
                              IFileService fileService,
                              IPersonService personService,
                              IInstallmentService installmentService)
        {
            BillService = billService;
            FileService = fileService;
            PersonService = personService;
            InstallmentService = installmentService;
        }

        public async virtual Task<IActionResult> Index(FilterViewModel<Bill> filter)
        {
            if (hasSession())
            {
                var billFilter = new BillFilter();

                if ((filter.ReferenceId.HasValue && filter.ReferenceId.IsPositive()) || filter.StartDate != null)
                {
                    billFilter.Pagination = await BillService.GetQueryablePagination(filter: x => x.PersonId == filter.ReferenceId && x.DueDate == (filter.StartDate ?? x.DueDate),
                                                                                           order: GetOrder(filter.Sort),
                                                                                           direction: filter.Direction,
                                                                                           page: filter.Page,
                                                                                           count: filter.Count);
                }
                else
                {
                    billFilter.Pagination = await BillService.GetQueryablePagination(order: GetOrder(filter.Sort), direction: filter.Direction, page: filter.Page, count: filter.Count);
                }

                billFilter.Filter = filter;

                return View(billFilter);
            }

            return Logout(Message.SessionExpired);
        }

        [Route(RouteManageId)]
        public async virtual Task<IActionResult> Manage(int id)
        {
            if (hasSession())
            {
                var model = new Bill();

                if (id.IsPositive())
                {
                    model = await BillService.Find(id);

                    if (model == null)
                    {
                        return NotFound();
                    }

                    SetManageInformation(model);
                }

                return View(model);
            }

            return Logout(Message.SessionExpired);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        [Route(RouteManageId)]
        public async virtual Task<IActionResult> Manage(Bill model)
        {
            if (hasSession())
            {
                SetManageInformation(model);

                if (ModelState.IsValid)
                {
                    try
                    {
                        model.UpdatedBy = base.GetCurrentUser();

                        var installment = HttpContext.Session.Get<Installment>(AppConfig.Session.Installment);

                        if (installment != null)
                        {
                            if (model.InstallmentId.IsPositive())
                            {
                                await InstallmentService.Manage(installment);
                            }
                            else
                            {
                                model.InstallmentId = (await InstallmentService.Insert(installment))?.Id;
                            }

                            model.InstallmentCount = installment.Dates.Count;
                        }
                        else
                        {
                            model.InstallmentCount = await InstallmentService.Count(model.InstallmentId);
                        }

                        model.PaymentDate = model.IsPaid && model.PaymentDate.HasValue && !model.PaymentDate.Equals(DateTime.MinValue) ? DateTime.Now : model.PaymentDate;

                        if (model.Id.IsPositive())
                        {
                            await BillService.Update(model);
                        }
                        else
                        {
                            model.CreatedBy = base.GetCurrentUser();

                            model = await BillService.Insert(model);
                        }

                        return RedirectToAction(nameof(Manage), new { id = model.Id }).WithSuccess(Message.SuccessOnSave);
                    }
                    catch
                    {
                        return View(nameof(Manage), model).WithError(Message.ErrorOnSave);
                    }
                }

                return View(nameof(Manage), model).WithWarning(Message.FillAllRequiredField);
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
                    var model = await BillService.Find(id);

                    try
                    {
                        if (model == null)
                        {
                            return RedirectToAction(nameof(Index)).WithWarning(Message.NotFound);
                        }

                        await BillService.Delete(model);

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

        private async void SetManageInformation(Bill model)
        {
            model.NotBillName = (await FileService.Find(model.BillId))?.Name;
            model.NotReceiptName = (await FileService.Find(model.ReceiptId))?.Name;

            model.NotName = model.PersonId.HasValue && model.PersonId.IsPositive() ? (await PersonService.GFind(model.PersonId.Value))?.Name : string.Empty;
        }

        private static Expression<Func<Bill, object>> GetOrder(int sort)
        {
            switch (sort)
            {
                case 1:
                    return x => x.DueDate;
            }

            return null;
        }
    }
}