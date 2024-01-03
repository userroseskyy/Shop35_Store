using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop35_Store.Domain.Entities.Account
{
   public class MyAccountInfoDto
    {
        public string Id{ get; set; }

        [Display(Name = "نام و نام خانوادگی")]
        public string FullName { get; set; }
        [Display(Name = "نام کاربری")]
        public string UserName { get; set; }
        [Display(Name = "ایمیل")]
        public string Email { get; set; }
        [Display(Name = "موبایل ")]
        public string PhoneNumber { get; set; }
        [Display(Name = "ایمیل تائید شد")]
        public bool EmailConfirmed { get; set; }
        [Display(Name = "موبایل تائید شد")]
        public bool PhoneNumberConfirmed { get; set;}
        [Display(Name = "ورود دو مرحله ای")]
        public bool TwoFactorEnabled{ get; set; }




    }
}
