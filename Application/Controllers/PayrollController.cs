using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Application.Classes;
using Application.Controllers.Base;
using Application.Models;
using Application.Models.Paycheck;
using Application.Models.Payroll;
using Domain.Entity;
using Microsoft.AspNetCore.Mvc;
using Service.Service.Interface;
using Tool.Extensions;
using Tool.Resources;
using Tool.Utilities;

namespace Application.Controllers
{
    [Route("folha-pagamento/")]
    public class PayrollController : BaseController
    {
        private readonly IPayrollService PaycheckService;
        private readonly IPersonRoleService PersonRoleService;
        private readonly IPaycheckService PaycheckPersonService;

        public PayrollController(IPayrollService paycheckService,
                                  IPersonRoleService personRoleService,
                                  IPaycheckService paycheckPersonService)
        {
            PaycheckService = paycheckService;
            PersonRoleService = personRoleService;
            PaycheckPersonService = paycheckPersonService;
        }

        public async virtual Task<IActionResult> Index(FilterViewModel<Payroll> filter)
        {
            if (hasSession())
            {
                var paycheckFilter = new PayrollFilter();

                if (filter.Model.Year > 0 || filter.Model.Month > 0 || filter.IsActivated() != null)
                {
                    paycheckFilter.Pagination = await PaycheckService.GetQueryablePagination(filter: x => (filter.Model.Year == 0 ? true : x.Year == filter.Model.Year) && (filter.Model.Month == EMonth.All ? true : x.Month == filter.Model.Month),
                                                                                             order: GetOrder(filter.Sort),
                                                                                             direction: filter.Direction,
                                                                                             page: filter.Page,
                                                                                             count: filter.Count);
                }
                else
                {
                    paycheckFilter.Pagination = await PaycheckService.GetQueryablePagination(order: GetOrder(filter.Sort), direction: filter.Direction, page: filter.Page, count: filter.Count);
                }

                paycheckFilter.Filter = filter;

                return View(paycheckFilter);
            }

            return Logout(Message.SessionExpired);
        }

        [Route(RouteManageId)]
        public async virtual Task<IActionResult> Manage(int id)
        {
            if (hasSession())
            {
                var model = new Payroll();

                if (id.IsPositive())
                {
                    model = await PaycheckService.Find(id);

                    if (model == null)
                    {
                        return NotFound();
                    }
                }

                return View(model);
            }

            return Logout(Message.SessionExpired);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        [Route(RouteManageId)]
        public async virtual Task<IActionResult> Manage(Payroll model)
        {
            if (hasSession())
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        var exist = await PaycheckService.Exist(model.Year ?? 0, model.Month);

                        if (!exist)
                        {
                            model.UpdatedBy = base.GetCurrentUser();

                            if (model.Id.IsPositive())
                            {
                                await PaycheckService.Update(model);
                            }
                            else
                            {
                                model.CreatedBy = base.GetCurrentUser();

                                model = await PaycheckService.Insert(model);
                            }

                            return RedirectToAction(nameof(Manage), new { id = model.Id }).WithSuccess(Message.SuccessOnSave);
                        }
                        else
                        {
                            return View(nameof(Manage), model).WithWarning(Message.DuplicateException);
                        }
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
                    var model = await PaycheckService.Find(id);

                    try
                    {
                        if (model == null)
                        {
                            return RedirectToAction(nameof(Index)).WithWarning(Message.NotFound);
                        }

                        await PaycheckService.Delete(model);

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

        private static Expression<Func<Payroll, object>> GetOrder(int sort)
        {
            switch (sort)
            {
                case 1:
                    return x => x.Year;
                case 2:
                    return x => x.Month;
            }

            return null;
        }
    }
}