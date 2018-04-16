using Microsoft.AspNet.Identity.EntityFramework;
using ProductTaskDay3.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ProductTaskDay3.ProductTask.DAL
{
    public class ProductDbContext: IdentityDbContext<ExtentedSignUp>
    {

            public ProductDbContext() : base("connection")
            {

            }
            public DbSet<Product> Products { get; set; }
            public DbSet<ProductImage> ProductImages { get; set; }

            protected override void OnModelCreating(DbModelBuilder modelBuilder)
            {
                base.OnModelCreating(modelBuilder);
            }

        
    }
}