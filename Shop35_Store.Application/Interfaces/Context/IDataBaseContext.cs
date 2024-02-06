using Microsoft.EntityFrameworkCore;
using Shop35_Store.Domain.Entities.HomePages;
using Shop35_Store.Domain.Entities.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop35_Store.Application.Interfaces.Context
{
    public interface IDataBaseContext
    {


        DbSet<Category> Categories { get; set; }

        DbSet<Product> Products { get; set; }
        DbSet<ProductImages> ProductImages { get; set; }
        DbSet<ProductFeatures> ProductFeatures { get; set; }
        DbSet<Slider>  sliders { get; set; }
        DbSet<HomePageImages>  HomePageImages { get; set; }



        int SaveChanges(bool acceptAllChangesOnSuccess);
        int SaveChanges();
        Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = new CancellationToken());
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken());

    }
}
