using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop35_Store.Domain.Entities.Users
{
    public class UserListDto
    {
        public string Id { get; set; }
        [Display(Name = "نام")]
        public string FirstName { get; set; }
        [Display(Name = "نام خانوادگی")]
        public string LastName { get; set; }
        [Display(Name = "نام کاربری")]
        public string UserName { get; set; }
        [Display(Name = "موبایل")]
        public string PhonNumber { get; set; }
        [Display(Name = "ایمیل تائید شد")]
        public bool EmailConfirmed { get; set; }
        [Display(Name = " تلاش  ورود ناموفق")]
        public int AccessFailedCount { get; set; }

    }
}
