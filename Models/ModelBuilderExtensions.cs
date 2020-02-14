using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Models
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BooksDisplayed>().HasData(
                     new BooksDisplayed() { Id = 1, BookGenre = "Comedy", BooksInStore = "Bossypants" },
                     new BooksDisplayed() { Id = 2, BookGenre = "Comedy", BooksInStore = "Yes please" },
                     new BooksDisplayed() { Id = 3, BookGenre = "Comedy", BooksInStore = "Me Talk Pretty One Day" },
                     new BooksDisplayed() { Id = 4, BookGenre = "Drama", BooksInStore = "Hamlet" },
                     new BooksDisplayed() { Id = 5, BookGenre = "Drama", BooksInStore = "Visul unei nopti de vara" },
                     new BooksDisplayed() { Id = 6, BookGenre = "Drama", BooksInStore = "Vanatorii de zmeie" },
                     new BooksDisplayed() { Id = 7, BookGenre = "Science-Fiction", BooksInStore = "Razboiul Lumilor" },
                     new BooksDisplayed() { Id = 8, BookGenre = "Science-Fiction", BooksInStore = "Solaris" },
                     new BooksDisplayed() { Id = 9, BookGenre = "Science-Fiction", BooksInStore = "The Left Hand of Darkness" },
                     new BooksDisplayed() { Id = 10, BookGenre = "Nature-Science", BooksInStore = "Walden" },
                     new BooksDisplayed() { Id = 11, BookGenre = "Nature-Science", BooksInStore = "Almanahul unui tinut de nisip" },
                     new BooksDisplayed() { Id = 12, BookGenre = "Nature-Science", BooksInStore = "H is for Hawk" }
            );
        }
    }
}
