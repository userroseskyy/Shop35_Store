
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace Shop35_Store.Domain.Entities.Roles
{
    public class AddUserRoleDto
    {
        [Display(Name = "نام  کاربر")]
        public string FullName { get; set; }
         [Display(Name = "ایمیل")]
         public string Email { get; set; }

        public string Id { get; set; }
        [Display(Name = "نام نقش")]
        public string Role { get; set; }
        [Display(Name = "انتخاب نقش")]
        public List<SelectListItem> Roles{ get; set; }
        

    }
}
