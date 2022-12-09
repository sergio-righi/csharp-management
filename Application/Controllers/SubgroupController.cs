using Application.Classes;
using Application.Controllers.Base;
using Application.Extensions;
using Application.Models;
using Application.Models.Subgroup;
using Domain.Entity;
using Microsoft.AspNetCore.Mvc;
using Service.Service.Interface;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Tool.Extensions;
using Tool.Resources;
using Tool.Utilities;

namespace Application.Controllers
{
    [Route("subgroup/")]
    public partial class SubgroupController : BaseController
    {
        private readonly IGroupService GroupService;
        private readonly ISubgroupService SubgroupService;

        public SubgroupController(IGroupService groupService,
                                  ISubgroupService subgroupService)
        {
            GroupService = groupService;
            SubgroupService = subgroupService;
        }

        [Route("{referenceId:int?}")]
        public async virtual Task<IActionResult> Index(FilterViewModel<Subgroup> filter)
        {
            if (hasSession())
            {
                if (filter.ReferenceId.IsNegativeOrZero())
                {
                    return NotFound();
                }

                var subgroupFilter = new SubgroupFilter();

                if (!string.IsNullOrWhiteSpace(filter.GetName()) || filter.Activated != null || filter.ReferenceId != null)
                {
                    subgroupFilter.Pagination = await SubgroupService.GetQueryablePagination(filter: x => x.GroupId == (filter.ReferenceId ?? x.GroupId) && x.Name.Contains(filter.GetName()) && x.IsActivated == (filter.IsActivated() ?? x.IsActivated),
                                                                                             order: GetOrder(filter.Sort),
                                                                                             direction: filter.Direction,
                                                                                             page: filter.Page,
                                                                                             count: filter.Count);
                }
                else
                {
                    subgroupFilter.Pagination = await SubgroupService.GetQueryablePagination(filter: x => x.IsActivated && x.GroupId == null, order: GetOrder(filter.Sort), direction: filter.Direction, page: filter.Page, count: filter.Count);
                }

                subgroupFilter.Filter = filter;

                return View(subgroupFilter);
            }

            return Logout(Message.SessionExpired);
        }

        [Route(RouteManage + "{groupId:int}/{id:int}")]
        public async virtual Task<IActionResult> Manage(int groupId, int id)
        {
            if (hasSession())
            {
                var model = new Subgroup();

                if (id.IsPositive())
                {
                    model = await SubgroupService.Find(id);

                    if (model == null)
                    {
                        return NotFound();
                    }
                }

                model.GroupId = groupId;

                return View(model);
            }

            return Logout(Message.SessionExpired);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        [Route(RouteManage + "{groupId:int}/{id:int}")]
        public async virtual Task<IActionResult> Manage(Subgroup model)
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
                            await SubgroupService.Update(model);
                        }
                        else
                        {
                            model.CreatedBy = base.GetCurrentUser();

                            model = await SubgroupService.Insert(model);
                        }

                        return RedirectToAction(nameof(Manage), new { id = model.Id, groupId = model.GroupId }).WithSuccess(Message.SuccessOnSave);
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
                    var model = await SubgroupService.Find(id);

                    try
                    {
                        if (model == null)
                        {
                            return RedirectToAction(nameof(Index)).WithWarning(Message.NotFound);
                        }

                        await SubgroupService.Delete(model);

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

        private static Expression<Func<Subgroup, object>> GetOrder(int sort)
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