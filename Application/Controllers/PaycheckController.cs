using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Application.Classes;
using Application.Controllers.Base;
using Application.Models;
using Application.Models.Paycheck;
using Domain.Entity;
using Domain.Entity.Specific;
using Microsoft.AspNetCore.Mvc;
using Service.Service.Interface;
using Tool.Extensions;
using Tool.Resources;
using Tool.Utilities;

namespace Application.Controllers
{
    [Route("contracheque/")]
    public class PaycheckController : FileController
    {
        private readonly IFileService FileService;
        private readonly IPersonService PersonService;
        private readonly IPayrollService PayrollService;
        private readonly IPaycheckService PaycheckService;

        public PaycheckController(IFileService fileService,
                                  IPersonService personService,
                                  IPayrollService payrollService,
                                  IPaycheckService paycheckService) : base(fileService)
        {
            FileService = fileService;
            PersonService = personService;
            PayrollService = payrollService;
            PaycheckService = paycheckService;
        }

        [Route("{referenceId:int}")]
        public async virtual Task<IActionResult> Index(FilterViewModel<SPaycheckPerson> filter)
        {
            if (hasSession())
            {
                var paycheckFilter = new PaycheckFilter();

                paycheckFilter.Pagination = await PaycheckService.SList(filter.ReferenceId ?? 0,
                                                                        filter.Name,
                                                                        order: GetOrder(filter.Sort),
                                                                        direction: filter.Direction,
                                                                        page: filter.Page,
                                                                        count: filter.Count);

                paycheckFilter.Filter = filter;

                return View(paycheckFilter);
            }

            return Logout(Message.SessionExpired);
        }

        [Route(RouteManage + "{payrollId:int}/{id:int}")]
        public async virtual Task<IActionResult> Manage(int payrollId, int id)
        {
            if (hasSession())
            {
                var model = new Paycheck()
                {
                    PayrollId = payrollId
                };

                if (id.IsPositive())
                {
                    model = await PaycheckService.Find(id);

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
        [Route(RouteManage + "{payrollId:int}/{id:int}")]
        public async virtual Task<IActionResult> Manage(Paycheck model)
        {
            if (hasSession())
            {
                SetManageInformation(model);

                if (ModelState.IsValid)
                {
                    try
                    {
                        var exist = model.IsExtra ? false : await PaycheckService.Exist(model.Id, model.PayrollId, model.PersonId, model.Week);

                        if (!exist)
                        {
                            model.UpdatedBy = base.GetCurrentUser();

                            model.FileId = await base.ManageFile(model.FileId, model.NotVoucher);

                            if (model.Id.IsPositive())
                            {
                                await PaycheckService.Update(model);
                            }
                            else
                            {
                                model.CreatedBy = base.GetCurrentUser();

                                model = await PaycheckService.Insert(model);
                            }

                            return RedirectToAction(nameof(Manage), new { id = model.Id, personId = model.PersonId, payrollId = model.PayrollId }).WithSuccess(Message.SuccessOnSave);
                        }
                        else
                        {
                            return View(nameof(Manage), model).WithWarning(Message.DuplicateException);
                        }
                    }
                    catch
                    {
                        /// should be improved to delete the file on error
                        return View(nameof(Manage), model).WithError(Message.ErrorOnSave);
                    }
                }

                return View(nameof(Manage), model).WithWarning(Message.FillAllRequiredField);
            }

            return Logout(Message.SessionExpired);
        }

        [Route(RouteDelete + "{payrollId:int}/{id:int}")]
        public async virtual Task<IActionResult> Delete(int payrollId, int id)
        {
            if (hasSession())
            {
                if (id.IsPositive())
                {
                    var model = await PaycheckService.Find(id);

                    try
                    {
                        if (model == null)
                        {
                            return RedirectToAction(nameof(Index), new { referenceId = payrollId }).WithWarning(Message.NotFound);
                        }

                        await PaycheckService.Delete(model);

                        return RedirectToAction(nameof(Index), new { referenceId = payrollId }).WithSuccess(Message.SuccessOnDelete);
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

        private async void SetManageInformation(Paycheck model)
        {
            model.NotName = model.PersonId.IsPositive() ? (await PersonService.GFind(model.PersonId))?.Name : string.Empty;
        }

        private static Expression<Func<SPaycheckPerson, object>> GetOrder(int sort)
        {
            switch (sort)
            {
                case 1:
                    return x => x.Name;
            }

            return null;
        }
    }
}