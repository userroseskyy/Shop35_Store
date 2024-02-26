using EndPoint.Site.Models;
using EndPoint.Site.Models.HomePage;
using Microsoft.AspNetCore.Mvc;
using Shop35_Store.Application.Interfaces.FacadPatern;
using Shop35_Store.Application.Service.Common.Queries.GetHomePageImages;
using Shop35_Store.Application.Service.Common.Queries.GetSlider;
using Shop35_Store.Application.Service.Products.FacadPattern;
using Shop35_Store.Application.Service.Products.Queries.GetProductForSite;
using System.Diagnostics;
using System.Reflection;

namespace EndPoint.Site.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly IGetSliderService _getSliderSevice;
        private readonly IGetHomePageService _homePageService;
        private readonly IProductFacad _productFacad;

        public HomeController(ILogger<HomeController> logger,IGetSliderService getSliderSevice, IGetHomePageService homePageService,IProductFacad  productFacad)
        {
            _logger = logger;
            _getSliderSevice = getSliderSevice;
            _homePageService = homePageService;
            _productFacad = productFacad;
        }


        public IActionResult Index()
        {
            HomePageViewModel homePage = new HomePageViewModel()
            {
                Sliders=_getSliderSevice.Execute().Data,
                pageImage=_homePageService.Execute().Data,
                Camera = _productFacad.GetProductForSiteService.Execute(ordering.theNewest, null, 1, 6, 15
                ).Data.Products,
                Mobile = _productFacad.GetProductForSiteService.Execute(ordering.theNewest, null, 1, 6, 1
                ).Data.Products,

            };
            return View(homePage);
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult Error2()
        {
            return View();
        }

    }
}
