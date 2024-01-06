using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop35_Store.Domain.Entities.Account
{
   public class ResetPasswordDto
    {
        [Required]
        [Display(Name ="پسورد جدید")]
        [DataType(DataType.Password)]

        public string Password { get; set; }
        [Display(Name = "تکرار پسورد جدید")]
        [DataType(DataType.Password)]
        [Compare(nameof(Password),ErrorMessage = "رمز و تکرار آن برابر نیست")]
        public string ConfirmPassword { get; set; }

        public string UserId { get; set; }


        public string Token { get; set; }
    }
}
