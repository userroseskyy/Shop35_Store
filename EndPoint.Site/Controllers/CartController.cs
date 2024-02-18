using Microsoft.AspNetCore.Mvc;
using Shop35_Store.Application.Service.Carts;
using Shop35_Store.Application.Utilities;

namespace EndPoint.Site.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService _cartService;
        private readonly CookiesManeger cookiesManeger;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
            cookiesManeger=new CookiesManeger();
        }
        public IActionResult Index()
        {
            var resultGetList=_cartService.GetMyCart(cookiesManeger.GetBrowserId(HttpContext));
            return View(resultGetList.Data);
        }
        public IActionResult AddToCart(long ProductId) 
        {
            

            var resultAdd = _cartService.AddToCart(ProductId, cookiesManeger.GetBrowserId(HttpContext));
            return RedirectToAction("Index");
        }
        public IActionResult Add(long CartItemId)
        {
            _cartService.Add(CartItemId);
            return RedirectToAction("index");
        }
        public IActionResult LowOff(long CartItemId)
        {
            _cartService.LowOff(CartItemId);
            return RedirectToAction("index");
        }
        public IActionResult Remove(long ProductId)
        {
            _cartService.removeFromCart(ProductId, cookiesManeger.GetBrowserId(HttpContext));
            return RedirectToAction("index");
        }
    }
}
