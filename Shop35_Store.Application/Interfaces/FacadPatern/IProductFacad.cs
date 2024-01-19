using Shop35_Store.Application.Service.Products.Command.AddNewCategory;
using Shop35_Store.Application.Service.Products.Command.AddNewProduct;
using Shop35_Store.Application.Service.Products.Queries.GetAllCategories;
using Shop35_Store.Application.Service.Products.Queries.GetCategories;
using Shop35_Store.Application.Service.Products.Queries.GetProductDetailForAdmin;
using Shop35_Store.Application.Service.Products.Queries.GetProductDetailForSite;
using Shop35_Store.Application.Service.Products.Queries.GetProductForAdmin;
using Shop35_Store.Application.Service.Products.Queries.GetProductForSite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop35_Store.Application.Interfaces.FacadPatern
{
    public interface IProductFacad
    {
        AddNewCattegoryService AddNewCattegoryService { get; }
        GetCategoriesService GetCategoriesService { get; }
        AddNewProductService AddNewProductService { get; }
        GetAllCategoriesService GetAllCategoriesService { get; }
        /// <summary>
        /// دریافت لیست محصولات
        /// </summary>
        IGetProductForSiteService GetProductForSiteService { get; }
        IGetProductDetailForSiteService GetProductDetailForSiteService { get; }

        IGetProductForAdminService GetProductForAdminService { get; }
        IGetProductDetailForAdminService GetProductDetailForAdminService { get; }
    }
}
