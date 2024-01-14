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
using System.Security.Claims;
using Shop35_Store.Domain.Entities.Roles;

// کنترلر مدیریت حساب کاربری
namespace EndPoint.Site.Areas.Admin.Controllers
{
    [Authorize(Roles = "Customer,Admin")]



    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly EmailSender _emailSender;
        private readonly RoleManager<Role> _roleManager;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, IConfiguration configuration, RoleManager<Role>  roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = new EmailSender(configuration);
            _roleManager = roleManager;
        }

        // صفحه اصلی حساب کاربری
       
       

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
        
        [HttpGet]
        public IActionResult TwoFactorEnabled()
        {
            var user = _userManager.FindByNameAsync(User.Identity.Name).Result;
            var result = _userManager.SetTwoFactorEnabledAsync(user, !user.TwoFactorEnabled).Result;
            return RedirectToAction(nameof(Index));
        }


        // صفحه ثبت‌ نام
        [AllowAnonymous]
        [HttpGet]
        public IActionResult Register()
        {
            return View();

        }
        [AllowAnonymous]
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
            var roleExists = _roleManager.RoleExistsAsync("Customer").Result;

            if (!roleExists)
            {
                
                return RedirectToAction("Error2", "home");  
            }

          
            var result = _userManager.CreateAsync(newUser, register.Password).Result;

           
             var addRoleResult = _userManager.AddToRoleAsync(newUser, "Customer").Result;
            if (addRoleResult.Succeeded) { 
              

                if (result.Succeeded) { 
                var token = _userManager.GenerateEmailConfirmationTokenAsync(newUser).Result;
                string callbakUrl = Url.Action("ConfirmEmail", "Account", new
                { UserId = newUser.Id, token = token }, protocol: Request.Scheme);
                string body =
                    $"لطفا برای فعال سازی حساب کاربری خود بر روی لینک زیر کلیک نمائید.<br> <a  href={callbakUrl}> لینک</a>";
                _emailSender.SendEmail(newUser.Email, body, "فعال سازی حساب کاربر");



                return RedirectToAction("DisplayEmail");}
            }
             

            string message = "";

            foreach (var item in result.Errors)
            {
                message += item.Description + Environment.NewLine;
            
            }

            TempData["Message"] = message;

            return View(register);

        }
        [AllowAnonymous]
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
        [AllowAnonymous]
        // نمایش پیغام دریافت ایمیل
        [HttpGet]
        public IActionResult DisplayEmail()
        {
            return View();
        }
        [AllowAnonymous]
        // صفحه ورود
        [HttpGet]
        public IActionResult Login(string returnUrl = "/")
        {
            return View(new UserLoginDto
            {
                ReturnUrl = returnUrl,
            });

        }
        [AllowAnonymous]
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

            if (result.IsLockedOut)
            {
                 _userManager.AccessFailedAsync(user);
                ModelState.AddModelError(string.Empty, "درخواست شما با خطا مواجه شد");
            }

            

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
                smsService.SendSmsAsync(user.PhoneNumber,smsCode);
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
        [AllowAnonymous]
        [HttpGet]
        public IActionResult LogOut()
        {
            _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        [AllowAnonymous]
        // صفحه فراموشی رمز عبور
        [HttpGet]
        public IActionResult ForgotPassword()
        {

            return View();
        }

        [AllowAnonymous]
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
        [AllowAnonymous]
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
        [AllowAnonymous]
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
        [AllowAnonymous]
        //صفحه  تایید تغییر پسورد
        [HttpGet]
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        // صفحه تنظیم شماره تلفن
        [HttpGet]
        
        public IActionResult SetPhoneNumber()
        {
            return View();
        }

        // پردازش صفحه تنظیم شماره تلفن
        [HttpPost]
       
        public IActionResult SetPhoneNumber(SetPhoneNumberDto phoneNumberDto)
        {
            var user = _userManager.FindByNameAsync(User.Identity.Name).Result;
           var setResult= _userManager.SetPhoneNumberAsync(user, phoneNumberDto.PhoneNumber).Result;
           string code = _userManager.GenerateChangePhoneNumberTokenAsync(user, phoneNumberDto.PhoneNumber).Result;

           SmsService smsService = new SmsService();
           smsService.SendSmsAsync(phoneNumberDto.PhoneNumber,code);
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
       
        public async Task<IActionResult> VerifyPhoneNumber(VerifyPhoneNumber verify)
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
               await _userManager.UpdateAsync(user);
            }
            return RedirectToAction("VerifySuccess");

          
        }
        // صفحه موفقیت تایید شماره تلفن
        [HttpGet]
        public IActionResult VerifySuccess()
        {
            return View();
        }
        [AllowAnonymous]
        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View() ;
        }
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> ExternalLogin(string ReturnUrl)
        {
            string url = Url.Action(nameof(CallBac),"Account",new
            {
                ReturnUrl
            });
            
            
            var propertis = _signInManager.ConfigureExternalAuthenticationProperties("Google", url);
            return new ChallengeResult("Google",propertis);
        }
        [AllowAnonymous]
        public  async Task<IActionResult> CallBac(string ReturnUrl)
        {
            var loginInfo = await _signInManager.GetExternalLoginInfoAsync();
            string email=loginInfo.Principal.FindFirst(ClaimTypes.Email).Value?? null;

            if (email==null)
            {
                return RedirectToAction("Home", "error2");
            }

            string firstName = loginInfo.Principal.FindFirst(ClaimTypes.GivenName)?.Value ?? null;
            string lastName=loginInfo.Principal.FindFirst(ClaimTypes.Surname)?.Value ?? null;

            var signin = await _signInManager.ExternalLoginSignInAsync("Google", loginInfo.ProviderKey,false, true);
            if (signin.Succeeded)
            {
                if(Url.IsLocalUrl(ReturnUrl))
                {
                    return Redirect("/");
                }
                return RedirectToAction("index","home");
            }
            else if (signin.RequiresTwoFactor) 
             {
                  return RedirectToAction("login","home");
            }
            var user= await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                User newUser=new User()
                {
                    UserName = email,
                    Email = email,
                    FirstName=firstName,
                    LastName=lastName,
                    EmailConfirmed=true,
                };

                var resultAddUser=_userManager.CreateAsync(newUser).Result;
                user=newUser;
               
                var addRoleResult = _userManager.AddToRoleAsync(newUser, "Customer").Result;

            }
            var resultAddLogin = _userManager.AddLoginAsync(user, loginInfo).Result;
            _signInManager.SignInAsync(user, false).Wait();
            return Redirect("/");
      
        }




    }
}
