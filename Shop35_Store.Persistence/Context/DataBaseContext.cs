using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Shop35_Store.Application.Interfaces.Context;
using Shop35_Store.Domain.Entities.Products;
using Shop35_Store.Domain.Entities.Roles;

using Shop35_Store.Domain.Entities.Users;

namespace Shop35_Store.Persistence.Context
{
    public class DataBaseContext : IdentityDbContext<User,Role,string>,IDataBaseContext
    {
        public DataBaseContext(DbContextOptions<DataBaseContext> Options):base(Options)
        {
                
        }

        public DbSet<Category> Categories { get ; set ; }
        public DbSet<Product> Products { get ; set ; }
        public DbSet<ProductImages> ProductImages { get; set ; }
        public DbSet<ProductFeatures> ProductFeatures { get; set ; }

        //public DbSet<Category> Categories { get ; set ; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IdentityUserLogin<string>>().HasKey(p => new { p.LoginProvider, p.ProviderKey });
            modelBuilder.Entity<IdentityUserRole<string>>().HasKey(p => new { p.UserId,p.RoleId });
          
            modelBuilder.Entity<IdentityUserToken<string>>().HasKey(p => new {p.UserId,p.LoginProvider,p.Name});
           



            modelBuilder.Entity<User>().HasQueryFilter(p => !p.IsRemoved);
            modelBuilder.Entity<Role>().HasQueryFilter(p => !p.IsRemoved);
            modelBuilder.Entity<Category>().HasQueryFilter(p => !p.IsRemoved);
        }
    }
}
