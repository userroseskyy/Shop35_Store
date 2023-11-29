using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop35_Store.Domain.Entities.Users
{
   public class UserLoginDto
    {

        [Required(ErrorMessage = "نام الزامی است")]
        
        [EmailAddress]
        [Display(Name = "ایمیل")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "پسورد الزامی است")]
        [DataType(DataType.Password)]
        [Display(Name = "پسورد ")]
        public string Password { get; set; }
        [Display(Name ="مرا بخاطر بسپار")]
        public bool IsPersistent { get; set; }=false;
        public  string ReturnUrl { get; set; }
    }
}
