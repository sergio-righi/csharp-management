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
using Application.Models.Overtime;
using Domain.Entity;
using Domain.Entity.Specific;
using Microsoft.AspNetCore.Mvc;
using Service.Service.Interface;
using Tool.Configurations;
using Tool.Extensions;
using Tool.Resources;
using Tool.Utilities;

namespace Application.Controllers
{
    [Route("hora-extra/")]
    public partial class OvertimeController : FileController
    {
        private readonly IFileService FileService;
        private readonly IPersonService PersonService;
        private readonly IOvertimeService OvertimeService;

        public OvertimeController(IFileService fileService,
                                  IPersonService personService,
                                  IOvertimeService overtimeService) : base(fileService)
        {
            FileService = fileService;
            PersonService = personService;
            OvertimeService = overtimeService;
        }

        public async virtual Task<IActionResult> Index(FilterViewModel<SOvertime> filter)
        {
            if (hasSession())
            {
                var overtimeFilter = new OvertimeFilter();

                overtimeFilter.Pagination = await OvertimeService.SList(filter.Model.PersonId, filter.Model.Date, order: GetOrder(filter.Sort), direction: filter.Direction, page: filter.Page, count: filter.Count);

                overtimeFilter.Filter = filter;

                return View(overtimeFilter);
            }

            return Logout(Message.SessionExpired);
        }

        [Route(RouteManageId)]
        public async virtual Task<IActionResult> Manage(int id)
        {
            if (hasSession())
            {
                var model = new Overtime();

                if (id.IsPositive())
                {
                    model = await OvertimeService.Find(id);

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
        public async virtual Task<IActionResult> Manage(Overtime model)
        {
            if (hasSession())
            {
                SetManageInformation(model);

                if (ModelState.IsValid)
                {
                    try
                    {
                        model.UpdatedBy = base.GetCurrentUser();

                        model.FileId = await base.ManageFile(model.FileId, model.NotFile);

                        model.PaymentDate = model.IsPaid && model.PaymentDate.HasValue && !model.PaymentDate.Equals(DateTime.MinValue) ? DateTime.Now : model.PaymentDate;

                        model.ConfirmationDate = model.IsConfirmed && model.ConfirmationDate.HasValue && !model.ConfirmationDate.Equals(DateTime.MinValue) ? DateTime.Now : model.ConfirmationDate;

                        if (model.Id.IsPositive())
                        {
                            await OvertimeService.Update(model);
                        }
                        else
                        {
                            model.CreatedBy = base.GetCurrentUser();

                            model = await OvertimeService.Insert(model);
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
                    var model = await OvertimeService.Find(id);

                    try
                    {
                        if (model == null)
                        {
                            return RedirectToAction(nameof(Index)).WithWarning(Message.NotFound);
                        }

                        await OvertimeService.Delete(model);

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

        private async void SetManageInformation(Overtime model)
        {
            model.NotName = model.PersonId.IsPositive() ? (await PersonService.GFind(model.PersonId))?.Name : string.Empty;
        }

        private static Expression<Func<SOvertime, object>> GetOrder(int sort)
        {
            switch (sort)
            {
                case 1:
                    return x => x.Name;
                case 2:
                    return x => x.Date;
            }

            return null;
        }
    }
}