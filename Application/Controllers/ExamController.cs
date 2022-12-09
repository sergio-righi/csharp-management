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
using Application.Models.Exam;
using Domain.Entity;
using Domain.Entity.Generic;
using Microsoft.AspNetCore.Mvc;
using Service.Service.Interface;
using Tool.Configurations;
using Tool.Extensions;
using Tool.Resources;
using Tool.Utilities;

namespace Application.Controllers
{
    [Route("exame/")]
    public partial class ExamController : FileController
    {
        private readonly IExamService ExamService;
        private readonly IFileService FileService;
        private readonly IPersonService PersonService;

        public ExamController(IExamService examService,
                              IFileService fileService,
                              IPersonService personService) : base(fileService)
        {
            ExamService = examService;
            FileService = fileService;
            PersonService = personService;
        }

        public async virtual Task<IActionResult> Index(FilterViewModel<GCustomInformation> filter)
        {
            if (hasSession())
            {
                var examFilter = new ExamFilter();

                examFilter.Pagination = await ExamService.GList(filter.Model.Id, filter.Model.Date, GetOrder(filter.Sort), filter.Direction, filter.Page, filter.Count);

                examFilter.Filter = filter;

                return View(examFilter);
            }

            return Logout(Message.SessionExpired);
        }

        [Route(RouteManageId)]
        public async virtual Task<IActionResult> Manage(int id)
        {
            if (hasSession())
            {
                var model = new Exam();

                if (id.IsPositive())
                {
                    model = await ExamService.Find(id);

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
        public async virtual Task<IActionResult> Manage(Exam model)
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

                        if (model.Id.IsPositive())
                        {
                            await ExamService.Update(model);
                        }
                        else
                        {
                            model.CreatedBy = base.GetCurrentUser();

                            model = await ExamService.Insert(model);
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
                    var model = await ExamService.Find(id);

                    try
                    {
                        if (model == null)
                        {
                            return RedirectToAction(nameof(Index)).WithWarning(Message.NotFound);
                        }

                        await ExamService.Delete(model);

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

        private async void SetManageInformation(Exam model)
        {
            model.NotName = model.PersonId.IsPositive() ? (await PersonService.GFind(model.PersonId))?.Name : string.Empty;
        }

        private static Expression<Func<GCustomInformation, object>> GetOrder(int sort)
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