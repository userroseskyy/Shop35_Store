using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shop35_Store.Domain.Entities.Roles;
using Shop35_Store.Domain.Entities.Users;
using System.Data;

namespace EndPoint.Site.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class RoleController : Controller
    {
        private readonly RoleManager<Role> _roleManager;
        private readonly UserManager<User> _userManager;

        public RoleController(RoleManager<Role> roleManager, UserManager<User> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var role = _roleManager.Roles.Select(p => new RoleListDto()
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,

            }).ToList();
            return View(role);

        }
        [HttpGet]
        public IActionResult Create()
        {

            return View();
        }
        [HttpPost]
        public IActionResult Create(AddNewRoleDto newRole)
        {
            Role role = new Role()
            {
                Description = newRole.Description,
                Name = newRole.Name,

            };
            var result = _roleManager.CreateAsync(role).Result;
            if (result.Succeeded)
            {
                return RedirectToAction("index", "Role", new { area = "Admin" });
            }
            ViewBag.Errors = result.Errors.ToList();
            return View(newRole);
        }
        [HttpGet]
        public IActionResult Edit(string Id)
        {
            var role = _roleManager.FindByIdAsync(Id).Result;
            EditRoleDto editRole = new EditRoleDto()
            {
                Id = role.Id,
                Description = role.Description,
                Name = role.Name,
            };

            return View(editRole);
        }
        [HttpPost]
        public IActionResult Edit(EditRoleDto editRole)
        {
            var role = _roleManager.FindByIdAsync(editRole.Id).Result;
            role.Id = editRole.Id;
            role.Description = editRole.Description;
            role.Name = editRole.Name;
            var result = _roleManager.UpdateAsync(role).Result;
            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Role", new { area = "Admin" });
            }
            ViewBag.Errors = result.Errors.ToList();

            return View();
        }

        [HttpGet]
        public IActionResult Details(string Id)
        {
            var role = _roleManager.FindByIdAsync(Id).Result;

            DetailRoleDto detailRole = new DetailRoleDto()
            {
                Id = role.Id,
                Description = role.Description,
                Name = role.Name,
            };
            return View(detailRole);

        }

        [HttpGet]
        public IActionResult Delete(string Id)
        {
            var role = _roleManager.FindByIdAsync(Id).Result;
            DeleteRoleDto deleteRoleDto = new DeleteRoleDto()
            {
                Id = role.Id,
                Description = role.Description,
                Name = role.Name,
            };
            return View(deleteRoleDto);

        }

        [HttpPost]
        public IActionResult Delete(DeleteRoleDto deletRole)
        {
            var role = _roleManager.FindByIdAsync(deletRole.Id).Result;
            var result = _roleManager.DeleteAsync(role).Result;
            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Role", new { area = "Admin" });
            }
            ViewBag.Errors = result.Errors.ToList();

            return View();

        }
        [HttpGet]
        public IActionResult UserInRole(string Name)
        {
            var userInRole = _userManager.GetUsersInRoleAsync(Name).Result;

            return View(userInRole.Select(p =>
            new UserListDto
            {
                FirstName = p.FirstName,
                LastName = p.LastName,
                UserName = p.UserName,
                PhonNumber = p.PhoneNumber,
                Id = p.Id

            }));
        }
        


    }
}
