using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop35_Store.Domain.Entities.Role
{
    public class Role:IdentityRole
    {
        public string Description { get; set; }
    }
}
