using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shop35_Store.Application.Interfaces.FacadPatern;

namespace EndPoint.Site.Areas.Admin.Controllers
{
    [Authorize(Roles = ("Admin"))]
    [Authorize(Roles = ("Customer"))]
    [Area("Admin")]
    public class CategoriesController : Controller
    {
        private readonly IProductFacad _productFacad;
        public CategoriesController(IProductFacad  productFacad)
        {
            _productFacad = productFacad;
        }

        public IActionResult Index(long? ParentId)
        {
          
            return View(_productFacad.GetCategoriesService.Execute(ParentId).Data);
        }
        [HttpGet]
        public IActionResult AddNewCaregory(long? ParentId)
        {
            ViewBag.ParentId = ParentId;
            return View();
        }
         [HttpPost]
        public IActionResult AddNewCategory(long? ParentId, string Name)
        {
            var result=_productFacad.AddNewCattegoryService.Execute(ParentId, Name);

            return Json(result);
        }
       

    }
}
