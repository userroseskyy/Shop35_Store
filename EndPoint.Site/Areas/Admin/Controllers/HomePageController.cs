using Microsoft.AspNetCore.Mvc;
using Shop35_Store.Application.Service.HomePage.AddHomePageImages;
using Shop35_Store.Domain.Entities.HomePages;

namespace EndPoint.Site.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomePageController : Controller
    {
        
       
            private readonly IAddHomePageImagesService _addHomePageImageService;
            public HomePageController(IAddHomePageImagesService addHomePageImageService)
            {
                _addHomePageImageService = addHomePageImageService;
            }
            public IActionResult Index()
            {
                return View();
            }
            [HttpGet]
            public IActionResult Add()
            {
                return View();
            }
            [HttpPost]
            public IActionResult Add(IFormFile file, string link, ImageLocation imageLocation)
            {
                _addHomePageImageService.Execute(new RequestAddHomePagesDto
                {
                    file = file,
                    ImageLocation = imageLocation,
                    link = link

                });

                return View();
            }
        }
    }

