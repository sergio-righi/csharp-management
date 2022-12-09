using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entity;
using Domain.Entity.Specific;
using Microsoft.AspNetCore.Mvc;
using Service.Service.Interface;
using Tool.Extensions;
using Tool.Utilities;

namespace Application.Controllers.Base
{
    public class ProductController : BaseController
    {
        private readonly IRentProductService RentProductService;
        private readonly ISaleProductService SaleProductService;

        public ProductController(IRentProductService rentProductService)
        {
            RentProductService = rentProductService;
        }

        public ProductController(ISaleProductService saleProductService)
        {
            SaleProductService = saleProductService;
        }

        public ProductController(IRentProductService rentProductService,
                                 ISaleProductService saleProductService)
        {
            RentProductService = rentProductService;
            SaleProductService = saleProductService;
        }

        protected async virtual Task<bool> ManageRent(int rentId, ICollection<SProduct> products)
        {
            if (!products.IsNullOrEmpty())
            {
                try
                {
                    ICollection<RentProduct> deleted = new List<RentProduct>();
                    ICollection<RentProduct> addModified = new List<RentProduct>();

                    foreach (var product in products)
                    {
                        if (product.Id.IsPositive())
                        {
                            var rentProduct = await RentProductService.Find(product.Id);

                            if (rentProduct != null)
                            {
                                if (product.IsDeleted())
                                {
                                    deleted.Add(rentProduct);
                                }
                                else
                                {
                                    rentProduct.Value = product.Value;
                                    rentProduct.Count = product.Count;
                                    rentProduct.UpdatedAt = DateTime.Now;
                                    rentProduct.UpdatedBy = GetCurrentUser();

                                    addModified.Add(rentProduct);
                                }
                            }
                        }
                        else
                        {
                            if (!product.IsDeleted())
                            {
                                addModified.Add(new RentProduct()
                                {
                                    RentId = rentId,
                                    Count = product.Count,
                                    UpdatedAt = DateTime.Now,
                                    CreatedAt = DateTime.Now,
                                    CreatedBy = GetCurrentUser(),
                                    UpdatedBy = GetCurrentUser(),
                                    ProductId = product.ProductId,
                                    Value = Util.GetCalculation(product.CalculationId, product.Value, product.Width, product.Height, product.Price, product.Count)
                                });
                            }
                        }
                    }

                    return await RentProductService.Manage(addModified, deleted);
                }
                catch
                {

                }
            }

            return false;
        }

        protected async virtual Task<bool> ManageSale(int saleId, ICollection<SProduct> products)
        {
            if (!products.IsNullOrEmpty())
            {
                try
                {
                    ICollection<SaleProduct> deleted = new List<SaleProduct>();
                    ICollection<SaleProduct> addModified = new List<SaleProduct>();

                    foreach (var product in products)
                    {
                        if (product.Id.IsPositive())
                        {
                            var saleProduct = await SaleProductService.Find(product.Id);

                            if (saleProduct != null)
                            {
                                if (product.IsDeleted())
                                {
                                    deleted.Add(saleProduct);
                                }
                                else
                                {
                                    saleProduct.Value = product.Value;
                                    saleProduct.Count = product.Count;
                                    saleProduct.UpdatedAt = DateTime.Now;
                                    saleProduct.UpdatedBy = GetCurrentUser();

                                    addModified.Add(saleProduct);
                                }
                            }
                        }
                        else
                        {
                            if (!product.IsDeleted())
                            {
                                addModified.Add(new SaleProduct()
                                {
                                    SaleId = saleId,
                                    Count = product.Count,
                                    Width = product.Width,
                                    Height = product.Height,
                                    UpdatedAt = DateTime.Now,
                                    CreatedAt = DateTime.Now,
                                    CreatedBy = GetCurrentUser(),
                                    UpdatedBy = GetCurrentUser(),
                                    ProductId = product.ProductId,
                                    Value = Util.GetCalculation(product.CalculationId, product.Value, product.Width, product.Height, product.Price, product.Count)
                                });
                            }
                        }
                    }

                    return await SaleProductService.Manage(addModified, deleted);
                }
                catch
                {

                }
            }

            return false;
        }
    }
}