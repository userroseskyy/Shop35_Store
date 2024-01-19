using Microsoft.AspNetCore.Mvc;
using Shop35_Store.Application.Interfaces.FacadPatern;

namespace EndPoint.Site.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductFacad _productFacad;

        public ProductsController(IProductFacad productFacad)
        {
            _productFacad = productFacad;
        }

        public IActionResult Index(string Searchkey, long? CatId = null, int page = 1)
        {
            return View(_productFacad.GetProductForSiteService.Execute(Searchkey, page, CatId).Data);
        }
        [HttpGet]
        public IActionResult Detail(long Id)
        {
            return View(_productFacad.GetProductDetailForSiteService.Execute(Id).Data);
        }
    }
}
