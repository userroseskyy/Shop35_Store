using Microsoft.AspNetCore.Identity;
using Shop35_Store.Domain.Entities.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Shop35_Store.Application.Helpers
{
    public class MyPasswordValidator : IPasswordValidator<User>
    {
        List<string> CommonPassword=new List<string>()
        {
           "123456","zxcV@34567","password","qwerty","123456789","qwerty","12345","1234567890","Aa123456","password1","000000","aa12345678"



        };
        public Task<IdentityResult> ValidateAsync(UserManager<User> manager, User user, string? password)
        {
            if (CommonPassword.Contains(password))
            {
                var result = IdentityResult.Failed(new IdentityError
                {
                    Code="CommonPassword",
                    Description="پسورد شما قابل شناسایی توسط ربات های هکر است!لطفا یک پسورد قوی انتخاب کنید",
                });
                return Task.FromResult(result);
            }
            if (string.Equals(user.UserName, password, StringComparison.OrdinalIgnoreCase))
            {
                return Task.FromResult(IdentityResult.Failed(new IdentityError
                {
                    Code = "UsernameAsPassword",
                    Description = "شما نمیتوانید  ایمیل را عنوان پسورد بگیرید"
                }));
            }
            return Task.FromResult(IdentityResult.Success);
        }
    }
}
