using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Application.Classes;
using Application.Controllers.Base;
using Application.Models;
using Application.Models.Expense;
using Domain.Entity;
using Microsoft.AspNetCore.Mvc;
using Service.Service.Interface;
using Tool.Extensions;
using Tool.Resources;

namespace Application.Controllers
{
    [Route("despesa/")]
    public class ExpenseController : FileController
    {
        private readonly IFileService FileService;
        private readonly IPersonService PersonService;
        private readonly IExpenseService ExpenseService;

        public ExpenseController(IFileService fileService,
                                 IPersonService personService,
                                 IExpenseService expenseService) : base(fileService)
        {
            FileService = fileService;
            PersonService = personService;
            ExpenseService = expenseService;
        }

        public async virtual Task<IActionResult> Index(FilterViewModel<Expense> filter)
        {
            if (hasSession())
            {
                var expenseFilter = new ExpenseFilter();

                if ((filter.ReferenceId.HasValue && filter.ReferenceId.IsPositive()) || filter.StartDate != null)
                {
                    expenseFilter.Pagination = await ExpenseService.GetQueryablePagination(filter: x => x.PersonId == filter.ReferenceId && x.Date == (filter.StartDate ?? x.Date),
                                                                                        order: GetOrder(filter.Sort),
                                                                                        direction: filter.Direction,
                                                                                        page: filter.Page,
                                                                                        count: filter.Count);
                }
                else
                {
                    expenseFilter.Pagination = await ExpenseService.GetQueryablePagination(order: GetOrder(filter.Sort), direction: filter.Direction, page: filter.Page, count: filter.Count);
                }

                expenseFilter.Filter = filter;

                return View(expenseFilter);
            }

            return Logout(Message.SessionExpired);
        }

        [Route(RouteManageId)]
        public async virtual Task<IActionResult> Manage(int id)
        {
            if (hasSession())
            {
                var model = new Expense();

                if (id.IsPositive())
                {
                    model = await ExpenseService.Find(id);

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
        public async virtual Task<IActionResult> Manage(Expense model)
        {
            if (hasSession())
            {
                SetManageInformation(model);

                if (ModelState.IsValid)
                {
                    try
                    {
                        model.UpdatedBy = base.GetCurrentUser();

                        model.ReceiptId = await base.ManageFile(model.ReceiptId, model.NotReceipt);

                        if (model.Id.IsPositive())
                        {
                            await ExpenseService.Update(model);
                        }
                        else
                        {
                            model.CreatedBy = base.GetCurrentUser();

                            model = await ExpenseService.Insert(model);
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
                    var model = await ExpenseService.Find(id);

                    try
                    {
                        if (model == null)
                        {
                            return RedirectToAction(nameof(Index)).WithWarning(Message.NotFound);
                        }

                        await ExpenseService.Delete(model);

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

        private async void SetManageInformation(Expense model)
        {
            model.NotName = model.PersonId.IsPositive() ? (await PersonService.GFind(model.PersonId))?.Name : string.Empty;
        }

        private static Expression<Func<Expense, object>> GetOrder(int sort)
        {
            switch (sort)
            {
                case 1:
                    return x => x.Date;
            }

            return null;
        }
    }
}