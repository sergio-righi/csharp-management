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
using System.Collections.Generic;
using System.Text;
using Tool.Utilities;
using System.Linq.Expressions;
using Application.Models.Product;

namespace Application.Controllers
{
    [Route("produto/")]
    public partial class ProductController : BaseController
    {
        private readonly IItemService ItemService;
        private readonly IProductService ProductService;

        public ProductController(IItemService itemService,
                                 IProductService productService)
        {
            ItemService = itemService;
            ProductService = productService;
        }

        [Route("{referenceId:int?}")]
        public async virtual Task<IActionResult> Index(FilterViewModel<Product> filter)
        {
            if (hasSession())
            {
                var productFilter = new ProductFilter();

                if (!string.IsNullOrWhiteSpace(filter.GetName()) || filter.Activated != null)
                {
                    productFilter.Pagination = await ProductService.GetQueryablePagination(filter: x => x.ItemId == (filter.ReferenceId ?? x.ItemId) && x.Name.Contains(filter.GetName()) && x.IsActivated == (filter.IsActivated() ?? x.IsActivated),
                                                                                           order: GetOrder(filter.Sort),
                                                                                           direction: filter.Direction,
                                                                                           page: filter.Page,
                                                                                           count: filter.Count);
                }
                else
                {
                    productFilter.Pagination = await ProductService.GetQueryablePagination(filter: x => x.ItemId == (filter.ReferenceId ?? x.ItemId), order: GetOrder(filter.Sort), direction: filter.Direction, page: filter.Page, count: filter.Count);
                }

                await SetIndexInformation(filter.ReferenceId);

                productFilter.Filter = filter;

                return View(productFilter);
            }

            return Logout(Message.SessionExpired);
        }

        [Route(RouteManageId + "{itemId:int?}")]
        public async virtual Task<IActionResult> Manage(int id, int? itemId)
        {
            if (hasSession())
            {
                var model = new Product();

                if (id.IsPositive())
                {
                    model = await ProductService.Find(id);

                    if (model == null)
                    {
                        return NotFound();
                    }
                }

                if (itemId.IsPositive())
                {
                    model.ItemId = itemId ?? 0;
                }

                await SetManageInformation(model);

                return View(model);
            }

            return Logout(Message.SessionExpired);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        [Route(RouteManageId + "{itemId:int?}")]
        public async virtual Task<IActionResult> Manage(Product model)
        {
            if (hasSession())
            {
                await SetManageInformation(model);

                if (ModelState.IsValid)
                {
                    try
                    {
                        model.UpdatedBy = base.GetCurrentUser();

                        if (model.Id.IsPositive())
                        {
                            await ProductService.Update(model);
                        }
                        else
                        {
                            model.CreatedBy = base.GetCurrentUser();

                            model = await ProductService.Insert(model);
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
                    var model = await ProductService.Find(id, false);

                    try
                    {
                        if (model == null)
                        {
                            return RedirectToAction(nameof(Index)).WithWarning(Message.NotFound);
                        }

                        model.IsDeleted = true;

                        await ProductService.Update(model);

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

        private async Task SetIndexInformation(int? itemId)
        {
            IEnumerable<Item> items;

            if (itemId.IsPositive())
            {
                items = (await ItemService.List(x => x.IsActivated && x.ItemId == itemId, null, includeProperties: nameof(Item.Parent))).ToList();
            }
            else
            {
                items = (await ItemService.List(x => x.IsActivated, null, includeProperties: nameof(Item.Parent))).ToList();
            }

            foreach (var item in items)
            {
                var auxiliar = item;

                while (true)
                {
                    item.NotName = string.IsNullOrWhiteSpace(item.NotName) ? auxiliar.Name : auxiliar.Name + " > " + item.NotName;

                    if (auxiliar.Parent == null)
                    {
                        break;
                    }

                    auxiliar = auxiliar.Parent;
                }
            }

            ViewBag.items = items;
        }

        private async Task SetManageInformation(Product model)
        {
            if (model.ItemId.IsPositive())
            {
                model.NotItemName = (await ItemService.Find(model.ItemId, null))?.Name;
            }
        }

        private static Expression<Func<Product, object>> GetOrder(int sort)
        {
            switch (sort)
            {
                case 1:
                    return x => x.Name;
                case 2:
                    return x => x.PurchasePrice;
                case 3:
                    return x => x.RentPrice;
                case 4:
                    return x => x.SalePrice;
                case 5:
                    return x => x.Count;
            }

            return null;
        }
    }
}