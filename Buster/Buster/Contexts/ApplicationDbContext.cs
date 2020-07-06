using Buster.Entities;
using Buster.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Buster.Contexts
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext>options)
            :base(options)
        {


        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            var roleAddmin = new IdentityRole
            {
                Id = "02d6bb5d-ce90-488c-8a35-0a8851ff9e94",
                Name = "admin",
                NormalizedName = "admin",
            };

            builder.Entity<IdentityRole>().HasData(roleAddmin);
            base.OnModelCreating(builder);
        }

        public DbSet<Product>Products { get; set; }
        public DbSet<Category>Categories { get; set; }
    }
}
