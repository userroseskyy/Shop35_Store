using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using NuGet.Common;
using Shop35_Store.Domain.Entities.Account;
using Shop35_Store.Domain.Entities.Users;
using Shop35_Store.Infrastructure.Service;
using System.Configuration;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Authorization;

// کنترلر مدیریت حساب کاربری
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

        // صفحه اصلی حساب کاربری
        [Authorize]
        [HttpGet]
        public IActionResult Index()
        {
           var user= _userManager.FindByNameAsync(User.Identity.Name).Result;
            MyAccountInfoDto myAccount = new MyAccountInfoDto()
            {
                Email = user.Email,
                EmailConfirmed = user.EmailConfirmed,
                FullName =$"{user.FirstName} {user.LastName}" ,
                Id= user.Id,
                PhoneNumber=user.PhoneNumber,
                PhoneNumberConfirmed=user.PhoneNumberConfirmed,
                TwoFactorEnabled=user.TwoFactorEnabled,
                UserName=user.UserName
                
            };


            return View(myAccount);
        }
        // فعال یا غیرفعال کردن Two-Factor Authentication برای یک کاربر
        [Authorize]
        [HttpGet]
        public IActionResult TwoFactorEnabled()
        {
            var user = _userManager.FindByNameAsync(User.Identity.Name).Result;
            var result = _userManager.SetTwoFactorEnabledAsync(user, !user.TwoFactorEnabled).Result;
            return RedirectToAction(nameof(Index));
        }


        // صفحه ثبت‌ نام

        [HttpGet]
        public IActionResult Register()
        {
            return View();

        }

        // پردازش ثبت‌ نام
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

        // تأیید ایمیل برای حساب کاربری
        [HttpGet]
        public IActionResult ConfirmEmail(string UserId, string Token)
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
           var result= _userManager.ConfirmEmailAsync(user, Token).Result;
           if (result.Succeeded)
           {
               //return RedirectToAction()
               
           }
           else
           {

           }


           return RedirectToAction("Login");

        }

        // نمایش پیغام دریافت ایمیل
        [HttpGet]
        public IActionResult DisplayEmail()
        {
            return View();
        }

        // صفحه ورود
        [HttpGet]
        public IActionResult Login(string returnUrl = "/")
        {
            return View(new UserLoginDto
            {
                ReturnUrl = returnUrl,
            });

        }

        // پردازش ورود
        [HttpPost]
        public IActionResult Login(UserLoginDto userLogin)
        {
            if (!ModelState.IsValid)
            {
                return View(userLogin);
            }

            var user = _userManager.FindByNameAsync(userLogin.UserName).Result;
            if (user==null)
            {
                ModelState.AddModelError(string.Empty, "کاربری با این نام ثبت نشده است");
                return View();
            }

            _signInManager.SignOutAsync();

            var result = _signInManager.PasswordSignInAsync(user, userLogin.Password, userLogin.IsPersistent, true).Result;
            if (result.Succeeded == true)
            {
                return Redirect(userLogin.ReturnUrl);
            }

            if (result.RequiresTwoFactor == true)
            {
                return RedirectToAction("TwoFactorLogin", new { userLogin.UserName, userLogin.IsPersistent });
            }

            //if (result.IsLockedOut)
            //{

            //}

            ModelState.AddModelError(string.Empty, "درخواست شما با خطا مواجه شد");

            return View();

        }
        // صفحه ورود دو مرحله‌ای
        [HttpGet]
        public IActionResult TwoFactorLogin(string UserName,bool IsPersistent)
        {
            var user=_userManager.FindByNameAsync(UserName).Result;
            if (user==null)
            {
                return BadRequest();
            }
            var providers=_userManager.GetValidTwoFactorProvidersAsync(user).Result;

            TwoFactorLoginDto model=new TwoFactorLoginDto();

            if (providers.Contains("Phone"))
            {
                string smsCode = _userManager.GenerateTwoFactorTokenAsync(user, "Phone").Result;
                SmsService smsService = new SmsService();
                smsService.Send(user.PhoneNumber,smsCode);
                model.Provider = "Phone";
                model.IsPersistent = IsPersistent;
            }
            else  if (providers.Contains("Email"))
            {
                string emailCode = _userManager.GenerateTwoFactorTokenAsync(user, "Email").Result;
                
                _emailSender.SendEmail(user.Email,$"Two Factore Code:{emailCode}", "Two Factor Login");
                model.Provider = "Email";
                model.IsPersistent = IsPersistent;

            }
            return View(model);


        }
        // پردازش ورود دو مرحله‌ای
        [HttpPost]
        public IActionResult TwoFactorLogin(TwoFactorLoginDto twoFactor)
        {
            if (!ModelState.IsValid)
            {
                return View(twoFactor);

            }
            var user = _signInManager.GetTwoFactorAuthenticationUserAsync().Result;
            if(user==null)
            {
                return BadRequest();
            }
            var result = _signInManager.TwoFactorSignInAsync(twoFactor.Provider,twoFactor.Code,twoFactor.IsPersistent,false).Result;
            if (result.Succeeded)
            {
                return RedirectToAction("index");
            }
            else if(result.IsLockedOut)
            {
                ModelState.AddModelError("", "حساب کاربری شما قفل شده است");
                return View();
            }
            else
            {
                //
            }

            return View(twoFactor);
        }

        // خروج از حساب کاربری

        [HttpGet]
        public IActionResult LogOut()
        {
            _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        // صفحه فراموشی رمز عبور
        [HttpGet]
        public IActionResult ForgotPassword()
        {

            return View();
        }
        // پردازش صفحه فراموشی رمز عبور
        [HttpPost]
        public IActionResult ForgotPassword(ForgotPasswordConfirmationDto forgot)
        {
            if (!ModelState.IsValid)
            {
                return View(forgot);
            }

            var user = _userManager.FindByEmailAsync(forgot.Email).Result;
            if (user==null || _userManager.IsEmailConfirmedAsync(user).Result==false)
            {
                ViewBag.message = "لینک تنظیم مجدد کلمه عبور برای ایمیل شما ارسال شد";
                return View();

            }
            string token=_userManager.GeneratePasswordResetTokenAsync(user).Result;
            string callbakUrl=Url.Action("ResetPassword","Account",new
            {
                UserId= user.Id,
                token=token
            },protocol:Request.Scheme);

            string body = $"برای تنظیم مجدد کلمه عبور بر روی لینک زیر کلیک کنید</br><a href={callbakUrl}> link reset Password </a>";
            _emailSender.SendEmail(user.Email,body,"فراموشی رمز عبور");
            ViewBag.message = "لینک تنظیم مجدد کلمه عبور برای ایمیل شما ارسال شد";


            return View();
        }
        // صفحه بازنشانی رمز عبور
        [HttpGet]
        public IActionResult ResetPassword(string UserId,string Token)
        {
            return View(new ResetPasswordDto()
            {
                Token = Token,
                UserId = UserId,

            });
        }
        // پردازش صفحه بازنشانی رمز عبور
        [HttpPost]
        public IActionResult ResetPassword(ResetPasswordDto reset)
        {
            if (!ModelState.IsValid)
            {
                return View(reset);
            }

            if (reset.Password !=reset.ConfirmPassword)
            {
                return BadRequest();
            }

            var user = _userManager.FindByIdAsync(reset.UserId).Result;
            if (user==null)
            {
                return BadRequest();
            }

            var result = _userManager.ResetPasswordAsync(user, reset.Token, reset.Password).Result;
            if (result.Succeeded)
            {

                return RedirectToAction(nameof(ResetPasswordConfirmation));

            }
            else 
            {
               ViewBag.Errors=result.Errors;
            }



                return View();
        }

        //صفحه  تایید تغییر پسورد
        [HttpGet]
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        // صفحه تنظیم شماره تلفن
        [HttpGet]
        [Authorize]
        public IActionResult SetPhoneNumber()
        {
            return View();
        }

        // پردازش صفحه تنظیم شماره تلفن
        [HttpPost]
        [Authorize]
        public IActionResult SetPhoneNumber(SetPhoneNumberDto phoneNumberDto)
        {
            var user = _userManager.FindByNameAsync(User.Identity.Name).Result;
           var setResult= _userManager.SetPhoneNumberAsync(user, phoneNumberDto.PhoneNumber).Result;
           string code = _userManager.GenerateChangePhoneNumberTokenAsync(user, phoneNumberDto.PhoneNumber).Result;

           SmsService smsService = new SmsService();
           smsService.Send(phoneNumberDto.PhoneNumber,code);
            TempData["PhoneNumber"]= phoneNumberDto.PhoneNumber;

            return RedirectToAction(nameof(VerifyPhoneNumber));
        }

        // صفحه تایید شماره تلفن
        [HttpGet]
        public IActionResult VerifyPhoneNumber()
        {
            return View(new VerifyPhoneNumber
            {
                PhoneNumber =  TempData["PhoneNumber"].ToString()
            });
        }
        // پردازش صفحه تایید شماره تلفن
        [HttpPost]
        [Authorize]
        public IActionResult VerifyPhoneNumber(VerifyPhoneNumber verify)
        {

            var user=_userManager.FindByNameAsync(User.Identity.Name).Result;
           bool resultVerify= _userManager.VerifyChangePhoneNumberTokenAsync(user, verify.Code, verify.PhoneNumber).Result;
            if (resultVerify==false)
            {
                ViewData["Message"] = $"کد وارد شده برای شماره{verify.PhoneNumber} اشتباه است.";
                return View(verify); 
            }
            else
            {
                user.PhoneNumberConfirmed = true;
                _userManager.UpdateAsync(user);
            }
            return RedirectToAction("VerifySuccess");

          
        }
        // صفحه موفقیت تایید شماره تلفن
        [HttpGet]
        public IActionResult VerifySuccess()
        {
            return View();
        }


    }
}
