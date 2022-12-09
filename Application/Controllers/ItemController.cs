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
using Tool.Utilities;
using System.Linq.Expressions;
using Application.Models.Item;

namespace Application.Controllers
{
    [Route("item/")]
    public partial class ItemController : BaseController
    {
        private readonly IItemService ItemService;

        public ItemController(IItemService itemService)
        {
            ItemService = itemService;
        }

        [Route("{referenceId:int?}")]
        public async virtual Task<IActionResult> Index(FilterViewModel<Item> filter)
        {
            if (hasSession())
            {
                if (filter.ReferenceId.IsNegativeOrZero())
                {
                    return NotFound();
                }

                var itemFilter = new ItemFilter();

                if (!string.IsNullOrWhiteSpace(filter.GetName()) || filter.Activated != null || filter.ReferenceId != null)
                {
                    itemFilter.Pagination = await ItemService.GetQueryablePagination(filter: x => !x.IsDeleted && x.ItemId == filter.ReferenceId && x.Name.Contains(filter.GetName()) && x.IsActivated == (filter.IsActivated() ?? x.IsActivated),
                                                                                     order: GetOrder(filter.Sort),
                                                                                     direction: filter.Direction,
                                                                                     page: filter.Page,
                                                                                     count: filter.Count);
                }
                else
                {
                    itemFilter.Pagination = await ItemService.GetQueryablePagination(filter: x => !x.IsDeleted && x.IsActivated && x.ItemId == filter.ReferenceId, order: GetOrder(filter.Sort), direction: filter.Direction, page: filter.Page, count: filter.Count);
                }

                itemFilter.Filter = filter;

                return View(itemFilter);
            }

            return Logout(Message.SessionExpired);
        }

        [Route(RouteManage + "{id:int}/{itemId:int?}")]
        public async virtual Task<IActionResult> Manage(int id, int? itemId)
        {
            if (hasSession())
            {
                var model = new Item();

                if (id.IsPositive())
                {
                    model = await ItemService.Find(id, false);

                    if (model == null)
                    {
                        return NotFound();
                    }
                }

                model.ItemId = itemId == 0 ? null : itemId;

                return View(model);
            }

            return Logout(Message.SessionExpired);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        [Route(RouteManage + "{id:int}/{itemId:int?}")]
        public async virtual Task<IActionResult> Manage(Item model)
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
                            await ItemService.Update(model);
                        }
                        else
                        {
                            model.CreatedBy = base.GetCurrentUser();

                            model = await ItemService.Insert(model);
                        }

                        return RedirectToAction(nameof(Manage), new { id = model.Id, itemId = model.ItemId }).WithSuccess(Message.SuccessOnSave);
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
                    var model = await ItemService.Find(id, false);

                    try
                    {
                        if (model == null)
                        {
                            return RedirectToAction(nameof(Index)).WithWarning(Message.NotFound);
                        }

                        model.IsDeleted = true;

                        await ItemService.Update(model);

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

        private static Expression<Func<Item, object>> GetOrder(int sort)
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