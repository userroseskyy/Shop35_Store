using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop35_Store.Domain.Entities.Account
{
   public class ForgotPasswordConfirmationDto
    {
        [Required]
        [EmailAddress]
        [Display(Name ="ایمیل")]
        public string Email { get; set; }   
    }
}
