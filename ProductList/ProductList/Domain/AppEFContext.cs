using Microsoft.EntityFrameworkCore;
using ProductList.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductList.Domain
{
    public class AppEFContext:DbContext
    {
        public AppEFContext(DbContextOptions<AppEFContext> options) :
              base(options)
        {

        }

        public DbSet<Product> Products { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }


    }
}
