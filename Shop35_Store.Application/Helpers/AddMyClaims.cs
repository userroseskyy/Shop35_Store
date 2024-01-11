using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Shop35_Store.Domain.Entities.Roles;
using Shop35_Store.Domain.Entities.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Shop35_Store.Application.Helpers
{
    public class AddMyClaims : UserClaimsPrincipalFactory<User>
    {
        public AddMyClaims(UserManager<User> userManager, IOptions<IdentityOptions> options) : base(userManager, options)
        {

        }
        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(User user)
        {
            var identity = await base.GenerateClaimsAsync(user);
            identity.AddClaim(new Claim("FullName", $"{user.FirstName} {user.LastName}"));


            return identity;
        }
    }
    public class AddClaim : IClaimsTransformation
    {
        public Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            if (principal != null)
            {
                var identity = principal.Identity as ClaimsIdentity;
                if (identity != null)
                {
                    identity.AddClaim(new Claim("TestClaim", "yes", ClaimValueTypes.String));
                    identity.AddClaim(new Claim("Credit", "11000", ClaimValueTypes.String));
                }

            }
            return Task.FromResult(principal);
        }
    }
}
