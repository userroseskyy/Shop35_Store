using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop35_Store.Domain.Entities.Users
{
    public class UserDeleteDto
    {
        public string Id { get; set; }
        [Display(Name = ("نام و نام خانوادگی"))]
        public string FullName { get; set; }
        [Display(Name = ("ایمیل"))]
        public string Email { get; set; }
        [Display(Name = ("نام کاربری"))]
        public string UserName { get; set; }



    }
}
