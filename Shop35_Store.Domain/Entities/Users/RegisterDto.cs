using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop35_Store.Domain.Entities.Users
{
   public class RegisterDto
    {
        [Required(ErrorMessage = "نام الزامی است")]
        [Display(Name = "نام")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "نام خانوادگی الزامی است")]
        [Display(Name = "نام خانوادگی")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "ایمیل الزامی است")]
        [EmailAddress]
        [Display(Name = "ایمیل")]
        public string Email{ get; set; }
        [Required(ErrorMessage = "رمز عبور الزامی است")]
        [DataType(DataType.Password)]
        [Display(Name = "رمز عبور")]
        public string Password { get; set; }
        [Required(ErrorMessage = "تکرار رمز عبورالزامی است")]
         [DataType(DataType.Password)]
        [Compare(nameof(Password))]
        [Display(Name = "تکرار رمز عبور")]
        public string ConFirmPassword{ get; set; }
       
    }
}
