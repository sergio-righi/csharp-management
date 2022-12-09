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
using Application.Classes.Attribute;
using Application.Models.JuridicalPerson;

namespace Application.Controllers
{
    [Route("pessoa-juridica/")]
    public partial class JuridicalPersonController : FileController
    {
        private readonly IFileService FileService;
        private readonly IPersonService PersonService;
        private readonly ISubgroupService SubgroupService;
        private readonly IJuridicalPersonService JuridicalPersonService;

        public JuridicalPersonController(IFileService fileService,
                                     IPersonService personService,
                                     ISubgroupService subgroupService,
                                     IJuridicalPersonService juridicalPersonService) : base(fileService)
        {
            FileService = fileService;
            PersonService = personService;
            SubgroupService = subgroupService;
            JuridicalPersonService = juridicalPersonService;
        }

        public async virtual Task<IActionResult> Index(FilterViewModel<JuridicalPerson> filter)
        {
            if (hasSession())
            {
                var juridicalPersonFilter = new JuridicalPersonFilter();

                if (!string.IsNullOrWhiteSpace(filter.GetName()) || !string.IsNullOrWhiteSpace(filter.GetCode()))
                {
                    juridicalPersonFilter.Pagination = await JuridicalPersonService.GetQueryablePagination(filter: x => !x.Person.IsDeleted && x.CompanyName.Contains(filter.GetName()) && x.CNPJ.Equals(filter.GetCode()),
                                                                                                           order: GetOrder(filter.Sort),
                                                                                                           direction: filter.Direction,
                                                                                                           page: filter.Page,
                                                                                                           count: filter.Count);
                }
                else
                {
                    juridicalPersonFilter.Pagination = await JuridicalPersonService.GetQueryablePagination(filter: x => !x.Person.IsDeleted, order: GetOrder(filter.Sort), direction: filter.Direction, page: filter.Page, count: filter.Count);
                }

                juridicalPersonFilter.Filter = filter;

                return View(juridicalPersonFilter);
            }

            return Logout(Message.SessionExpired);
        }

        [Route(RouteManageId)]
        public async virtual Task<IActionResult> Manage(int id)
        {
            if (hasSession())
            {
                var model = new JuridicalPerson();

                if (id.IsPositive())
                {
                    model = await JuridicalPersonService.Find(id);

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
        public async virtual Task<IActionResult> Manage(JuridicalPerson model)
        {
            if (hasSession())
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        model.CNPJ = model.CNPJ.RemoveMask();

                        model.UpdatedBy = base.GetCurrentUser();

                        var photoId = await base.ManageFile(model.Person.PhotoId, model.Person.NotPhoto, FileManagement.Avatar.Write);

                        if (model.Id.IsPositive())
                        {
                            model.Person = await PersonService.Find(model.Id);

                            model.Person.PhotoId = photoId;
                            model.Person.UpdatedBy = base.GetCurrentUser();

                            await JuridicalPersonService.Update(model);
                        }
                        else
                        {
                            model.CreatedBy = base.GetCurrentUser();

                            model.Person.IsActivated = true;
                            model.Person.PhotoId = photoId;
                            model.Person.TypeId = EPerson.Juridical;
                            model.Person.CreatedBy = base.GetCurrentUser();
                            model.Person.UpdatedBy = base.GetCurrentUser();

                            model = await JuridicalPersonService.Insert(model);
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

        private static Expression<Func<JuridicalPerson, object>> GetOrder(int sort)
        {
            switch (sort)
            {
                case 1:
                    return x => x.CompanyName;
            }

            return null;
        }
    }
}