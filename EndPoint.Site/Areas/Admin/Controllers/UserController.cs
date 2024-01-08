using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Shop35_Store.Domain.Entities.Roles;
using Shop35_Store.Domain.Entities.Users;
using Shop35_Store.Infrastructure.Service;
using System.Configuration;

namespace EndPoint.Site.Areas.Admin.Controllers
{

    [Authorize(Roles = "Customer")]
    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    public class UserController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly   RoleManager<Role> _roleManager;
        private readonly EmailSender _emailSender;

        public UserController(UserManager<User> userManager, RoleManager<Role> roleManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager= roleManager;
            _emailSender = new EmailSender(configuration);
        }

       
        public IActionResult Index()
        {
            var users = _userManager.Users
                 .Select(p=> new UserListDto()
            {
                Id= p.Id,
            FirstName = p.FirstName,
            LastName = p.LastName,
            UserName = p.UserName,
            PhonNumber=p.PhoneNumber,
            EmailConfirmed = p.EmailConfirmed,
            AccessFailedCount = p.AccessFailedCount

            }).ToList();
            return View(users);
        }
        

        [HttpGet]
        public IActionResult Edit(string Id)
        {
            var user = _userManager.FindByIdAsync(Id).Result;

            UserEditDto userEditDto= new UserEditDto()
            {
                Email = user.Email,
                FirstName = user.FirstName,
                lastName = user.LastName,
                Id = user.Id,
                PhoneNumber = user.PhoneNumber,
                UserName=user.UserName,
            };
            return View(userEditDto);
        }

        [HttpPost]
        public IActionResult Edit(UserEditDto userEdit)
        {
            var user = _userManager.FindByIdAsync(userEdit.Id).Result;
            user.FirstName=userEdit.FirstName;
            user.LastName = userEdit.lastName;
            user.Email = userEdit.Email;
            user.PhoneNumber = userEdit.PhoneNumber;
            user.UserName = userEdit.UserName;

           var result=_userManager.UpdateAsync(user).Result;
           if (result.Succeeded)
           {
              return  RedirectToAction("Index", "User", new { area = "Admin" });
            }
           string message = "";
           foreach (var item in result.Errors.ToList())
           {
               message += item.Description + Environment.NewLine;
           }
           TempData["Message"] = message;

            return View(userEdit);

        }
        [HttpGet]
        public IActionResult Delete(string Id)
        {
        
            
            var user=_userManager.FindByIdAsync(Id).Result;
         UserDeleteDto userDelete = new UserDeleteDto()
         {
             Id=user.Id,
             FullName = $"{user.FirstName} {user.LastName}",

             Email = user.Email,
             UserName=user.UserName,
         };
         return View(userDelete);   
        } 
        [HttpPost]
        public IActionResult Delete(UserDeleteDto userDelete)
        {
            var user = _userManager.FindByIdAsync(userDelete.Id).Result;

            user.RemoveTime=DateTime.Now;
            user.IsRemoved=true;
           var result= _userManager.UpdateAsync(user).Result;
            if (user.IsRemoved==true)
            {

                return RedirectToAction("Index", "User", new { area = "Admin" });
              }
        string message = "";
            foreach (var item in result.Errors.ToList())
        {
            message += item.Description + Environment.NewLine;
        }

        TempData["Message"] = message;
            return View(userDelete);  

        }
        [HttpGet]
        public IActionResult Detail(string Id)
        {
            var user = _userManager.FindByIdAsync(Id).Result;

            UserDetailDto userDetail = new UserDetailDto()
            {
                Email = user.Email,
                FirstName = user.FirstName,
                lastName = user.LastName,
                Id = user.Id,
                PhoneNumber = user.PhoneNumber,
                UserName = user.UserName,
            };
            return View(userDetail);
        }

        [HttpGet]
        public IActionResult AddUserRole(string Id)
        {
            var user=_userManager.FindByIdAsync(Id).Result;
           var roles=new List<SelectListItem>(
               _roleManager.Roles.Select(p=> new SelectListItem
               {
                   Text = p.Name,
                   Value = p.Name,

               })).ToList();
            


            return View(new AddUserRoleDto
            {
                Id=Id,
                Roles=roles,
                Email=user.Email,
                FullName=$"{user.FirstName} {user.LastName}",
            });
        }
        [HttpPost]
        public IActionResult AddUserRole(AddUserRoleDto newUserRole)
        {
            var user = _userManager.FindByIdAsync(newUserRole.Id).Result;

            var result = _userManager.AddToRoleAsync(user, newUserRole.Role).Result;

           return RedirectToAction("UserRoles","User",new{Id=user.Id, area="Admin"});
        }

        [HttpGet]
        public IActionResult UserRoles(string Id)
        {
            var user = _userManager.FindByIdAsync(Id).Result;
            var roles = _userManager.GetRolesAsync(user).Result;
            TempData["UserId"] = Id;
            ViewBag.UserInfo = $"نام کاربر : {user.FirstName} {user.LastName} ایمیل کاربر: {user.Email}";
            return View(roles);


        }
        

            [HttpPost]
            public IActionResult DeleteRole(string role)
            {
            var Id = TempData["UserId"]?.ToString();
            var user =_userManager.FindByIdAsync(Id).Result;
            
            var result =  _userManager.RemoveFromRoleAsync(user, role).Result;

            return Json(new { success = true });
            }

        







    }
}
