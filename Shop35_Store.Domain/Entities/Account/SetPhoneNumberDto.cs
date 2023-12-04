using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop35_Store.Domain.Entities.Account
{
    public class SetPhoneNumberDto
    {
        [Required]
        [RegularExpression(@"(\+98|0)?9\d{9}")]
        [Display(Name ="شماره موبایل")]
        public string PhoneNumber { get; set; }
    }
}
