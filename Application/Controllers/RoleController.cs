using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Classes;
using Application.Controllers.Base;
using Application.Models.Role;
using Application.Services.Interface;
using Domain.Entity;
using Domain.Entity.Generic;
using Domain.Entity.Specific;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using Service.Service.Interface;
using Tool.Extensions;
using Tool.Resources;
using Tool.Utilities;

namespace Application.Controllers
{
    [Route("cargo/")]
    public partial class RoleController : BaseController
    {
        private readonly IPersonService PersonService;
        private readonly IViewRenderService ViewRenderService;
        private readonly IPersonRoleService PersonRoleService;

        public RoleController(IPersonService personService,
                              IViewRenderService viewRenderService,
                              IPersonRoleService personRoleService)
        {
            PersonService = personService;
            ViewRenderService = viewRenderService;
            PersonRoleService = personRoleService;
        }

        public async virtual Task<IActionResult> Index()
        {
            if (hasSession())
            {
                return View();
            }

            return Logout(Message.SessionExpired);
        }

        [Route(RouteManageId)]
        public async virtual Task<IActionResult> Manage(ERole id)
        {
            if (hasSession())
            {
                var model = new RolePeopleModel();

                model.People = await PersonRoleService.SList(id, true);

                model.RolePerson.RoleId = id;

                return View(model);
            }

            return Logout(Message.SessionExpired);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        [Route(RouteManageId)]
        public async virtual Task<IActionResult> Manage(RolePeopleModel model)
        {
            if (hasSession())
            {
                if (ModelState.IsValid)
                {
                    if (!model.People.IsNullOrEmpty())
                    {
                        try
                        {
                            ICollection<PersonRole> added = new List<PersonRole>();
                            ICollection<PersonRole> modified = new List<PersonRole>();

                            foreach (var item in model.People)
                            {
                                var personRole = await PersonRoleService.SFind(item.PersonId, item.RoleId, true);

                                if (personRole == null)
                                {
                                    if (!item.IsDeleted())
                                    {
                                        var newPersonRole = SetPersonRole(item);

                                        newPersonRole.UpdatedAt = DateTime.Now;
                                        newPersonRole.UpdatedBy = base.GetCurrentUser();

                                        newPersonRole.CreatedAt = DateTime.Now;
                                        newPersonRole.CreatedBy = base.GetCurrentUser();

                                        added.Add(newPersonRole);
                                    }
                                }
                                else
                                {
                                    if (item.IsDeleted())
                                    {
                                        var oldPersonRole = SetPersonRole(item);

                                        oldPersonRole.EndDate = DateTime.Now;
                                        oldPersonRole.UpdatedAt = DateTime.Now;

                                        oldPersonRole.UpdatedBy = base.GetCurrentUser();

                                        modified.Add(oldPersonRole);
                                    }
                                }
                            }

                            await PersonRoleService.Manage(added, modified);

                            return RedirectToAction(nameof(Manage), new { id = model.RolePerson.RoleId }).WithSuccess(Message.SuccessOnSave);
                        }
                        catch
                        {
                            return View(nameof(Manage), model).WithError(Message.ErrorOnSave);
                        }
                    }
                }

                return View(nameof(Manage), model).WithWarning(Message.FillAllRequiredField);
            }

            return Logout(Message.SessionExpired);
        }

        [HttpPost]
        public async virtual Task<JsonResult> AddPerson(RoleModel model)
        {
            if (hasSession())
            {
                if (model.PersonId.IsPositive())
                {
                    var personRole = await PersonRoleService.SFind(model.PersonId, model.RoleId, true);

                    if (personRole == null)
                    {
                        var contentHTML = await ViewRenderService.RenderToString("~/Views/Role/_Person.cshtml", new SRolePerson
                        {
                            EndDate = null,
                            Name = model.NotName,
                            RoleId = model.RoleId,
                            StartDate = DateTime.Now,
                            PersonId = model.PersonId
                        });

                        return Json(new ResponseJSON() { IsSuccess = true, Content = contentHTML });
                    }
                    else
                    {
                        return Json(new ResponseJSON() { IsSuccess = false, Message = Message.DuplicateException });
                    }
                }

                return Json(new ResponseJSON() { IsSuccess = false, Message = Message.NotFound });
            }

            return Json(new ResponseJSON() { IsSuccess = false, Message = Message.SessionExpired });
        }

        private PersonRole SetPersonRole(SRolePerson rolePerson)
        {
            return new PersonRole()
            {
                Id = rolePerson.Id,
                UpdatedAt = DateTime.Now,
                RoleId = rolePerson.RoleId,
                EndDate = rolePerson.EndDate,
                PersonId = rolePerson.PersonId,
                StartDate = rolePerson.StartDate,
                UpdatedBy = base.GetCurrentUser()
            };
        }
    }
}