using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop35_Store.Domain.Entities.Account
{
    public class VerifyPhoneNumber
    {
      
        public string PhoneNumber { get; set; }
        [Required]
        [MinLength(6)] 
        [MaxLength(6)]
        public string Code { get; set; }
    }
}
