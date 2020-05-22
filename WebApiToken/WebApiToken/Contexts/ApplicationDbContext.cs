using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiToken.Models;

namespace WebApiToken.Contexts
{
    public class ApplicationDbContext: IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext>options)
            :base(options)
        {
                
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            var roleAdmin = new IdentityRole()
            {
                Id = "f6bd4a2b-6700-436d-a620-93ace0028dc6",
                Name = "admin",
                NormalizedName = "admin",
            };
            builder.Entity<IdentityRole>().HasData(roleAdmin);


            base.OnModelCreating(builder);
           
        }
    }
}
