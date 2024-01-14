using Shop35_Store.Application.Service.Products.Command.AddNewCategory;
using Shop35_Store.Application.Service.Products.Command.AddNewProduct;
using Shop35_Store.Application.Service.Products.Queries.GetAllCategories;
using Shop35_Store.Application.Service.Products.Queries.GetCategories;
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
        GetAllCategoriesService GetAllCategoriesService {  get; }
    }
}
