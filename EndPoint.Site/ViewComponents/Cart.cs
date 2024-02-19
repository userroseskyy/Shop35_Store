using Microsoft.AspNetCore.Mvc;
using Shop35_Store.Application.Service.Carts;
using Shop35_Store.Application.Utilities;

namespace EndPoint.Site.ViewComponents
{
    public class Cart : ViewComponent
    {
        private readonly ICartService _cartService;
        private readonly CookiesManeger _cookiesManeger;
        public Cart(ICartService  cartService)
        {
            _cartService = cartService;
            _cookiesManeger = new CookiesManeger();
        }
        public IViewComponentResult Invoke()
        {
            return View(viewName: "Cart", _cartService.GetMyCart(_cookiesManeger.GetBrowserId(HttpContext)).Data);
        }

    }
}
