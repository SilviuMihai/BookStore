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
                     new BooksDisplayed() { BookId = 1, BookGenre = "Comedy", BooksInStore = "Bossypants" },
                     new BooksDisplayed() { BookId = 2, BookGenre = "Comedy", BooksInStore = "Yes please" },
                     new BooksDisplayed() { BookId = 3, BookGenre = "Comedy", BooksInStore = "Me Talk Pretty One Day" },
                     new BooksDisplayed() { BookId = 4, BookGenre = "Drama", BooksInStore = "Hamlet" },
                     new BooksDisplayed() { BookId = 5, BookGenre = "Drama", BooksInStore = "Visul unei nopti de vara" },
                     new BooksDisplayed() { BookId = 6, BookGenre = "Drama", BooksInStore = "Vanatorii de zmeie" },
                     new BooksDisplayed() { BookId = 7, BookGenre = "Science-Fiction", BooksInStore = "Razboiul Lumilor" },
                     new BooksDisplayed() { BookId = 8, BookGenre = "Science-Fiction", BooksInStore = "Solaris" },
                     new BooksDisplayed() { BookId = 9, BookGenre = "Science-Fiction", BooksInStore = "The Left Hand of Darkness" },
                     new BooksDisplayed() { BookId = 10, BookGenre = "Nature-Science", BooksInStore = "Walden" },
                     new BooksDisplayed() { BookId = 11, BookGenre = "Nature-Science", BooksInStore = "Almanahul unui tinut de nisip" },
                     new BooksDisplayed() { BookId = 12, BookGenre = "Nature-Science", BooksInStore = "H is for Hawk" }
            );
        }
    }
}
