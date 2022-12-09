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
using Tool.Utilities;
using Tool.Extensions;
using System.Linq.Expressions;
using Application.Models.NaturalPerson;
using System.Security.Cryptography.Xml;
using System.Collections.Generic;

namespace Application.Controllers
{
    [Route("pessoa-fisica/")]
    public partial class NaturalPersonController : FileController
    {
        private readonly IFileService FileService;
        private readonly IPersonService PersonService;
        private readonly INaturalPersonService NaturalPersonService;
        private readonly IPersonDocumentService PersonDocumentService;

        public NaturalPersonController(IFileService fileService,
                                       IPersonService personService,
                                       INaturalPersonService naturalPersonService,
                                       IPersonDocumentService personDocumentService) : base(fileService)
        {
            FileService = fileService;
            PersonService = personService;
            NaturalPersonService = naturalPersonService;
            PersonDocumentService = personDocumentService;
        }

        public async virtual Task<IActionResult> Index(FilterViewModel<NaturalPerson> filter)
        {
            if (hasSession())
            {
                var naturalPersonFilter = new NaturalPersonFilter();

                if (!string.IsNullOrWhiteSpace(filter.GetName()) || !string.IsNullOrWhiteSpace(filter.GetCode()))
                {
                    naturalPersonFilter.Pagination = await NaturalPersonService.GetQueryablePagination(filter: x => !x.Person.IsDeleted && (x.FullName.Contains(filter.GetName())) || x.Person.Documents.Any(x => x.Number.Equals(filter.GetCode())),
                                                                                                       order: GetOrder(filter.Sort),
                                                                                                       direction: filter.Direction,
                                                                                                       page: filter.Page,
                                                                                                       count: filter.Count,
                                                                                                       includeProperties: nameof(Person.Documents));
                }
                else
                {
                    naturalPersonFilter.Pagination = await NaturalPersonService.GetQueryablePagination(filter: x => !x.Person.IsDeleted, order: GetOrder(filter.Sort), direction: filter.Direction, page: filter.Page, count: filter.Count);
                }

                naturalPersonFilter.Filter = filter;

                return View(naturalPersonFilter);
            }

            return Logout(Message.SessionExpired);
        }

        [Route(RouteManageId)]
        public async virtual Task<IActionResult> Manage(int id)
        {
            if (hasSession())
            {
                var model = new NaturalPerson();

                if (id.IsPositive())
                {
                    model = await NaturalPersonService.Find(id);

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
        public async virtual Task<IActionResult> Manage(NaturalPerson model)
        {
            if (hasSession())
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        model.FullName = $"{model.GivenName} {model.Surname}";

                        var photoId = await base.ManageFile(model.Person.PhotoId, model.Person.NotPhoto, FileManagement.Avatar.Write);

                        if (model.Id.IsPositive())
                        {
                            model.UpdatedAt = DateTime.Now;
                            model.UpdatedBy = base.GetCurrentUser();

                            model.Person = await PersonService.Find(model.Id);

                            model.Person.PhotoId = photoId;
                            model.Person.UpdatedBy = base.GetCurrentUser();

                            await NaturalPersonService.Update(model);
                        }
                        else
                        {
                            model.CreatedAt = DateTime.Now;
                            model.CreatedBy = base.GetCurrentUser();

                            model.Person.IsActivated = true;
                            model.Person.PhotoId = photoId;
                            model.Person.TypeId = EPerson.Natural;
                            model.Person.CreatedBy = base.GetCurrentUser();
                            model.Person.UpdatedBy = base.GetCurrentUser();

                            model = await NaturalPersonService.Insert(model);
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

        private static Expression<Func<NaturalPerson, object>> GetOrder(int sort)
        {
            switch (sort)
            {
                case 1:
                    return x => x.GivenName;
            }

            return null;
        }
    }
}