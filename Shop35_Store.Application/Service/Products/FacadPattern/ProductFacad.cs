using Microsoft.AspNetCore.Hosting;
using Shop35_Store.Application.Interfaces.Context;
using Shop35_Store.Application.Interfaces.FacadPatern;
using Shop35_Store.Application.Service.Products.Command.AddNewCategory;
using Shop35_Store.Application.Service.Products.Command.AddNewProduct;
using Shop35_Store.Application.Service.Products.Queries.GetAllCategories;
using Shop35_Store.Application.Service.Products.Queries.GetCategories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop35_Store.Application.Service.Products.FacadPattern
{

    public class ProductFacad : IProductFacad
    {
        private readonly IDataBaseContext _context;
        private readonly IHostingEnvironment _environment;
        public ProductFacad(IDataBaseContext context,IHostingEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        private AddNewCattegoryService _addNewCategory;

        public AddNewCattegoryService AddNewCattegoryService
        {
            get
            {
                return _addNewCategory = _addNewCategory ?? new AddNewCattegoryService(_context);
            }
        }

        private GetCategoriesService _getCategoriesService;
        public GetCategoriesService GetCategoriesService
        {
            get
            {
                return _getCategoriesService = _getCategoriesService ?? new GetCategoriesService(_context);
            }
        }

        public AddNewProductService _addNewProductService;
        public AddNewProductService AddNewProductService
        {
            get
            {
                return _addNewProductService = _addNewProductService ?? new AddNewProductService(_context,_environment);
            }
        }

        public GetAllCategoriesService _getAllCategoriesService;
        public GetAllCategoriesService GetAllCategoriesService
        {
            get
            {
            return  _getAllCategoriesService= _getAllCategoriesService ?? new  GetAllCategoriesService(_context);
            }
        }
    }
}
