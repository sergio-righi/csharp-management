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
using Tool.Extensions;
using System.Linq.Expressions;
using Application.Models.Group;

namespace Application.Controllers
{
    [Route("grupo/")]
    public partial class GroupController : BaseController
    {
        private readonly IGroupService GroupService;

        public GroupController(IGroupService groupService)
        {
            GroupService = groupService;
        }

        [Route("")]
        public async virtual Task<IActionResult> Index(FilterViewModel<Group> filter)
        {
            if (hasSession())
            {
                var groupFilter = new GroupFilter();

                if (!string.IsNullOrWhiteSpace(filter.GetName()) || filter.Activated != null)
                {
                    groupFilter.Pagination = await GroupService.GetQueryablePagination(filter: x => x.Name.Contains(filter.GetName()) && x.IsActivated == (filter.IsActivated() ?? x.IsActivated),
                                                                                       order: GetOrder(filter.Sort),
                                                                                       direction: filter.Direction,
                                                                                       page: filter.Page,
                                                                                       count: filter.Count);
                }
                else
                {
                    groupFilter.Pagination = await GroupService.GetQueryablePagination(order: GetOrder(filter.Sort), direction: filter.Direction, page: filter.Page, count: filter.Count);
                }

                groupFilter.Filter = filter;

                return View(groupFilter);
            }

            return Logout(Message.SessionExpired);
        }

        [Route(RouteManageId)]
        public async virtual Task<IActionResult> Manage(int id)
        {
            if (hasSession())
            {
                var model = new Group();

                if (id.IsPositive())
                {
                    model = await GroupService.Find(id);

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
        public async virtual Task<IActionResult> Manage(Group model)
        {
            if (hasSession())
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        model.UpdatedBy = base.GetCurrentUser();

                        if (model.Id.IsPositive())
                        {
                            await GroupService.Update(model);
                        }
                        else
                        {
                            model.CreatedBy = base.GetCurrentUser();

                            model = await GroupService.Insert(model);
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
                    var model = await GroupService.Find(id);

                    try
                    {
                        if (model == null)
                        {
                            return RedirectToAction(nameof(Index)).WithWarning(Message.NotFound);
                        }

                        await GroupService.Delete(model);

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

        private static Expression<Func<Group, object>> GetOrder(int sort)
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