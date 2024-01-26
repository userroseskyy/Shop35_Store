using EndPoint.Site.Models;
using EndPoint.Site.Models.HomePage;
using Microsoft.AspNetCore.Mvc;
using Shop35_Store.Application.Service.Common.Queries.GetSlider;
using System.Diagnostics;

namespace EndPoint.Site.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly IGetSliderService _getSliderSevice;
        public HomeController(ILogger<HomeController> logger,IGetSliderService getSliderSevice)
        {
            _logger = logger;
            _getSliderSevice = getSliderSevice;
        }

        public IActionResult Index()
        {
            HomePageViewModel homePage = new HomePageViewModel()
            {
                Sliders=_getSliderSevice.Execute().Data,
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
