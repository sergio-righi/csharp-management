using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Application.Classes;
using Application.Controllers.Base;
using Application.Extensions;
using Application.Models;
using Application.Models.Search;
using Application.Models.Search.Product;
using Domain.Entity;
using Domain.Entity.Generic;
using Domain.Entity.Specific;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Service.Service.Interface;
using Tool.Configurations;
using Tool.Extensions;
using Tool.Resources;
using Tool.Utilities;

namespace Application.Controllers
{
    [Route("pesquisar-produto/")]
    public class SearchProductController : Base.ProductController
    {
        private readonly IProductService ProductService;
        private readonly IRentProductService RentProductService;
        private readonly ISaleProductService SaleProductService;

        public string SessionKey
        {
            get { return HttpContext.Session.Get<string>(AppConfig.Session.Key); }
            set { HttpContext.Session.Set(AppConfig.Session.Key, value); }
        }

        public IList<SProduct> SessionProduct
        {
            get { return HttpContext.Session.Get<IList<SProduct>>(AppConfig.Session.Product); }
            set { HttpContext.Session.Set(AppConfig.Session.Product, value); }
        }

        public SearchProductController(IProductService productService,
                                       IRentProductService rentProductService,
                                       ISaleProductService saleProductService) : base(rentProductService, saleProductService)
        {
            ProductService = productService;
            RentProductService = rentProductService;
            SaleProductService = saleProductService;
        }

        [Route("{controllerId}/{toolId:int}")]
        public async virtual Task<IActionResult> Index(EController controllerId, int toolId)
        {
            if (hasSession())
            {
                SessionKey = Cryptography.GetEncrypted($"{toolId}{controllerId.ToString().ToLower()}");

                await SetSessionInformation(controllerId, toolId);

                var configuration = new SearchProductConfig()
                {
                    Id = toolId,
                    SessionKey = SessionKey,
                    ControllerId = controllerId
                };

                return await Search(new SearchProductFilter()
                {
                    Configuration = configuration
                });
            }

            return Logout(Message.SessionExpired);
        }

        [Route("listar/")]
        public async virtual Task<IActionResult> Search(SearchProductFilter model)
        {
            if (hasSession())
            {
                var searchProductModel = new SearchProductFilter();

                searchProductModel.Pagination = await ProductService.SList(model.Filter.Name, model.Configuration.ControllerId, true, false, model.Filter.Page, model.Filter.Count);

                switch (model.Configuration.ControllerId)
                {
                    case EController.Rent:
                        searchProductModel.Pagination.ForEach(x => x.RentId = model.Configuration.Id);
                        break;
                    case EController.Sale:
                        searchProductModel.Pagination.ForEach(x => x.SaleId = model.Configuration.Id);
                        break;
                }

                searchProductModel.Filter = model.Filter;
                searchProductModel.Products = SessionProduct;
                searchProductModel.Configuration = model.Configuration;

                return View("~/Views/Search/Product/Index.cshtml", searchProductModel);
            }

            return Logout(Message.SessionExpired);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        [Route("salvar/")]
        public async virtual Task<IActionResult> Save(SearchProductFilter model)
        {
            if (hasSession())
            {
                try
                {
                    switch (model.Configuration.ControllerId)
                    {
                        case EController.Rent:
                            await base.ManageRent(model.Configuration.Id, model.Products);
                            break;
                        case EController.Sale:
                            await base.ManageSale(model.Configuration.Id, model.Products);
                            break;
                    }
                }
                catch
                {

                }

                return PartialView("~/Views/Search/Product/_Resume.cshtml", SessionProduct);
            }

            return Logout(Message.SessionExpired);
        }

        [HttpPost]
        [Route("adicionar/")]
        public async virtual Task<IActionResult> Add(SProduct model)
        {
            if (hasSession())
            {
                var products = SessionProduct;

                var product = products.FirstOrDefault(x => x.ProductId == model.ProductId && (model.Width.HasValue ? x.Width == model.Width : true) && (model.Height.HasValue ? x.Height == model.Height : true));

                if (product != null)
                {
                    product.Value = null;
                    product.Count += model.Count;
                }
                else
                {
                    model.Key = Token.Get(16).ToLower();

                    products.Add(model);
                }

                SessionProduct = products;

                return PartialView("~/Views/Search/Product/_Resume.cshtml", SessionProduct);
            }

            return Logout(Message.Logout);
        }

        [HttpGet]
        [Route(RouteDelete + "{key:alpha}")]
        public async virtual Task<IActionResult> Remove(string key)
        {
            if (hasSession())
            {
                var products = SessionProduct;

                var product = products.FirstOrDefault(x => x.Key.Equals(key));

                if (product != null)
                {
                    products.Remove(product);
                }

                SessionProduct = products;

                return PartialView("~/Views/Search/Product/_Resume.cshtml", SessionProduct);
            }

            return Logout(Message.Logout);
        }

        [HttpGet]
        [Route("limpar/")]
        public async virtual Task<IActionResult> Reset()
        {
            if (hasSession())
            {
                var products = SessionProduct;

                products.RemoveAll(x => x.Id.IsZero());

                SessionProduct = products;

                return PartialView("~/Views/Search/Product/_Resume.cshtml", SessionProduct);
            }

            return Logout(Message.Logout);
        }

        [HttpPost]
        [Route("sincronizar/{controllerId}/{id:int}")]
        public async virtual Task<IActionResult> Sync(EController controllerId, int id)
        {
            if (hasSession())
            {
                await SetSessionInformation(controllerId, id);

                return PartialView("~/Views/Search/Product/_Resume.cshtml", SessionProduct);
            }

            return Logout(Message.Logout);
        }

        private async Task SetSessionInformation(EController controllerId, int id)
        {
            switch (controllerId)
            {
                case EController.Rent:
                    SessionProduct = (await RentProductService.SList(id)).ToList();
                    break;
                case EController.Sale:
                    SessionProduct = (await SaleProductService.SList(id)).ToList();
                    break;
            }
        }
    }
}