using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Shop35_Store.Application.Interfaces.Context;

using Shop35_Store.Domain.Entities.Roles;

using Shop35_Store.Domain.Entities.Users;

namespace Shop35_Store.Persistence.Context
{
    public class DataBaseContext : IdentityDbContext<User,Role,string>,IDataBaseContext
    {
        public DataBaseContext(DbContextOptions<DataBaseContext> Options):base(Options)
        {
                
        }

    }
}
