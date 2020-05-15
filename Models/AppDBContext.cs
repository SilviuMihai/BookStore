using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace BookStore.Models
{
    public class AppDBContext : IdentityDbContext<ApplicationUser> 
        //if we look at what inherits IdentityDbContext, it is IdentityUser
        //but we are adding new properties, thats why we created AplicationUser class which inherits from IdentityUser,
        //keeping the default properties from IdentityUser, but also adding new ones.
    {
        public AppDBContext(DbContextOptions<AppDBContext> options):base(options)
        {

        }
        //public DbSet<UserProfile> TheUsersProfile { get; set; }
        public DbSet<BooksDisplayed> BooksInStore { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Seed();
        }
    }
}
