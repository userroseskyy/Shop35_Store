using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop35_Store.Application.Helpers
{
   public class UserCreditRequerment:IAuthorizationRequirement
    {
        public int Credit { get; set; }
        public UserCreditRequerment(int credit)
        {
            Credit=credit;
        }

    }
    public class UserCreditHandler : AuthorizationHandler<UserCreditRequerment>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, UserCreditRequerment requirement)
        {
            var claim = context.User.FindFirst("Credit");
            if (claim != null)
            {
                int credit=int.Parse(claim?.Value);
                if(credit >=requirement.Credit)
                {
                    context.Succeed(requirement);
                }
            }
            return Task.CompletedTask;
           
        }
    }
}
