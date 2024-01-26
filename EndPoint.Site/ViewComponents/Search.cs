using Microsoft.AspNetCore.Mvc;
using Shop35_Store.Application.Service.Common.Queries.GetCategory;
using Shop35_Store.Application.Service.Products.Queries.GetCategories;

namespace EndPoint.Site.ViewComponents
{
    public class Search:ViewComponent
    {
        private readonly IGetCategoryService _getCategoryService;

        public Search(IGetCategoryService getCategoryService)
        {
            _getCategoryService = getCategoryService;
        }
        public IViewComponentResult Invoke()
        {
            return View(viewName: "Search", _getCategoryService.Execute().Data);
        }
    }
}
