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
        //public DbSet<UserWithBooksDB> UserBooksConnectionDB { get; set; }
        public DbSet<BooksDisplayed> BooksInStore { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Seed();


            modelBuilder.Entity<BooksDisplayed>().HasKey(c => new { c.BookId });

            //modelBuilder.Entity<UserWithBooksDB>().HasKey(c => new { c.UserBooksId });

        }
    }
}
/*
 * How to Revert a Database using Package Manager Console: 
 * If you have problems in the database and want to be deleted, just revert to a known working database, by looking into migrations
 * Example:
 * Migration 1,2,3,4,5
 * 4 and 5 failed in the database.
 * 3 is a stable one.
 * Steps:
 * Update-Database 3 (now the database was revert it)
 * Now Migrations can be removed
 * Remove-Migration 5
 * Remove-migration 4
 * 
 * This is good fix.
 */