
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shop35_Store.Domain.Entities.Users;
using System.Security.Claims;

namespace EndPoint.Site.Controllers
{ 
    [Authorize]
    public class UserClaimController : Controller
    {
        private UserManager<User> _userManager;
        public UserClaimController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

      
        public IActionResult Index()
        {
            return View(User.Claims);
        }

        [HttpGet]
        public IActionResult Create()
        
        
        {
            return View();
        }
          [HttpPost]
        public IActionResult Create(string ClaimType,string ClaimValue)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Error = "لطفاً تمام فیلدها را پر کنید.";
                return View("create");
            }
            var user = _userManager.GetUserAsync(User).Result;

            Claim newClaim=new Claim(ClaimType,ClaimValue,ClaimValueTypes.String);
            if (newClaim==null)
            {
                return View();
            }
            
            var result=_userManager.AddClaimAsync(user,newClaim).Result;
            if (result.Succeeded)
            {
                return RedirectToAction("index");
            }
            else
            {
                foreach (var item in result.Errors) 
                {
                    ModelState.AddModelError("", item.Description);
                }
            }
            return View();
        }
        [HttpGet]
        public IActionResult Delete(string ClaimType)
        {
            var user=_userManager.GetUserAsync(User).Result;
            Claim claim=User.Claims.Where(p=> p.Type==ClaimType).FirstOrDefault();
            if (claim!=null)
            {
                var result=_userManager.RemoveClaimAsync(user,claim).Result;
                if (result.Succeeded)
                {
                    return RedirectToAction("index");
                }
            }
          return RedirectToAction("index");  
        }

    }
}
