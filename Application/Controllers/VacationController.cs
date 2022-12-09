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
using Application.Models.Vacation;
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
    [Route("ferias/")]
    public partial class VacationController : FileController
    {
        private readonly IFileService FileService;
        private readonly IPersonService PersonService;
        private readonly IVacationService VacationService;

        public VacationController(IFileService fileService,
                                  IPersonService personService,
                                  IVacationService vacationService) : base(fileService)
        {
            FileService = fileService;
            PersonService = personService;
            VacationService = vacationService;
        }

        public async virtual Task<IActionResult> Index(FilterViewModel<SVacation> filter)
        {
            if (hasSession())
            {
                var vacationFilter = new VacationFilter();

                vacationFilter.Pagination = await VacationService.SList(filter.Model.PersonId, filter.Model.Year, GetOrder(filter.Sort), filter.Direction, filter.Page, filter.Count);

                vacationFilter.Filter = filter;

                return View(vacationFilter);
            }

            return Logout(Message.SessionExpired);
        }

        [Route(RouteManageId)]
        public async virtual Task<IActionResult> Manage(int id)
        {
            if (hasSession())
            {
                var model = new Vacation();

                if (id.IsPositive())
                {
                    model = await VacationService.Find(id);

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
        public async virtual Task<IActionResult> Manage(Vacation model)
        {
            if (hasSession())
            {
                SetManageInformation(model);

                if (ModelState.IsValid)
                {
                    try
                    {
                        model.UpdatedBy = base.GetCurrentUser();

                        model.FileId = await base.ManageFile(model.FileId, model.NotVoucher);

                        if (model.Id.IsPositive())
                        {
                            await VacationService.Update(model);
                        }
                        else
                        {
                            model.CreatedBy = base.GetCurrentUser();

                            model = await VacationService.Insert(model);
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
                    var model = await VacationService.Find(id);

                    try
                    {
                        if (model == null)
                        {
                            return RedirectToAction(nameof(Index)).WithWarning(Message.NotFound);
                        }

                        await VacationService.Delete(model);

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

        private async void SetManageInformation(Vacation model)
        {
            model.NotName = model.PersonId.IsPositive() ? (await PersonService.GFind(model.PersonId))?.Name : string.Empty;
        }

        private static Expression<Func<SVacation, object>> GetOrder(int sort)
        {
            switch (sort)
            {
                case 1:
                    return x => x.Name;
                case 2:
                    return x => x.Year;
            }

            return null;
        }
    }
}