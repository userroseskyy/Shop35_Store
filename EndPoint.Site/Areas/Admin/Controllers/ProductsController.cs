using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Shop35_Store.Application.Interfaces.FacadPatern;

namespace EndPoint.Site.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductsController : Controller
    {
        private readonly IProductFacad _productFacad;
        public ProductsController(IProductFacad productFacad)
        {
            _productFacad = productFacad;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult AddNewProduct()
        {
            ViewBag.Categories = new SelectList(_productFacad.GetAllCategoriesService.Execute().Data, "Id", "Name");
            return View();
        }
        //[HttpPost]
        //public IActionResult AddNewProduct()
        //{
        //    ViewBag.Categories = new SelectList(_productFacad.GetAllCategoriesService.Execute().Data, "Id", "Name");
        //    return View();
        //}

    }
}
