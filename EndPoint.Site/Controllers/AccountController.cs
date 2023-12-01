using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using NuGet.Common;
using Shop35_Store.Domain.Entities.Users;
using Shop35_Store.Infrastructure.Service;
using System.Configuration;


namespace EndPoint.Site.Areas.Admin.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly EmailSender _emailSender;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = new EmailSender(configuration);
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();

        }
        [HttpPost]
        public IActionResult Register(RegisterDto register)
        {
            if (ModelState.IsValid == false)
            {
                return View(register);
            }
            User newUser = new User()
            {
                FirstName = register.FirstName,
                LastName = register.LastName,
                Email = register.Email,
                UserName = register.Email,

            };
            var result = _userManager.CreateAsync(newUser, register.Password).Result;


            if (result.Succeeded)
            {
                var token = _userManager.GenerateEmailConfirmationTokenAsync(newUser).Result;
                string callbakUrl = Url.Action("ConfirmEmail", "Account", new
                { UserId = newUser.Id, token = token }, protocol: Request.Scheme);
                string body =
                    $"لطفا برای فعال سازی حساب کاربری خود بر روی لینک زیر کلیک نمائید.<br> <a  href={callbakUrl}> link</a>";
                _emailSender.SendEmail(newUser.Email, body, "فعال سازی حساب کاربر");



                return RedirectToAction("DisplayEmail");
            }


            string message = "";

            foreach (var item in result.Errors)
            {
                message += item.Description + Environment.UserName;
            }

            TempData["Message"] = message;

            return View(register);

        }

        [HttpGet]
        public IActionResult ConfirmEmail(string UserId, string token)
        {
            if (UserId == null || Token == null)
            {
                return BadRequest();

            }
            var user = _userManager.FindByIdAsync(UserId).Result;
            if (user == null)
            {
                return View("Error");
            }
            return View();

        }


        [HttpGet]
        public IActionResult DisplayEmail()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login(string returnUrl = "/")
        {
            return View(new UserLoginDto
            {
                ReturnUrl = returnUrl,
            });

        }
        [HttpPost]
        public IActionResult Login(UserLoginDto userLogin)
        {
            if (!ModelState.IsValid)
            {
                return View(userLogin);
            }

            var user = _userManager.FindByNameAsync(userLogin.UserName).Result;

            _signInManager.SignOutAsync();

            var result = _signInManager.PasswordSignInAsync(user, userLogin.Password, userLogin.IsPersistent, true).Result;
            if (result.Succeeded == true)
            {
                return Redirect(userLogin.ReturnUrl);
            }

            //if (!result.RequiresTwoFactor == true)
            //{
            //    return Redirect(userLogin.ReturnUrl);
            //}

            //if (result.IsLockedOut)
            //{

            //}

            ModelState.AddModelError(string.Empty, "درخواست شما با خطا مواجه شد");

            return View();

        }

        public IActionResult LogOut()
        {
            _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
