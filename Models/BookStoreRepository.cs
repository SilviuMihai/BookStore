using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Models
{
    public class BookStoreRepository : IBookStore
    {
        private readonly List<BooksDisplayed> _getBooks;
        public BookStoreRepository()
        { 
            _getBooks = new List<BooksDisplayed>()
            {
            new BooksDisplayed(){Id=1 ,BookGenre="Comedy",BooksInStore="Bossypants" },
            new BooksDisplayed(){Id=2 ,BookGenre="Comedy",BooksInStore="Yes please" },
            new BooksDisplayed(){Id=3 ,BookGenre="Comedy",BooksInStore="Me Talk Pretty One Day" },
            new BooksDisplayed(){Id=1 ,BookGenre="Drama",BooksInStore="Hamlet" },
            new BooksDisplayed(){Id=2 ,BookGenre="Drama",BooksInStore="Visul unei nopti de vara" },
            new BooksDisplayed(){Id=3 ,BookGenre="Drama",BooksInStore="Vanatorii de zmeie" },
            new BooksDisplayed(){Id=1 ,BookGenre="Science-Fiction",BooksInStore="Razboiul Lumilor" },
            new BooksDisplayed(){Id=2 ,BookGenre="Science-Fiction",BooksInStore="Solaris" },
            new BooksDisplayed(){Id=3 ,BookGenre="Science-Fiction",BooksInStore="The Left Hand of Darkness" },
            new BooksDisplayed(){Id=1 ,BookGenre="Nature-Science",BooksInStore="Walden" },
            new BooksDisplayed(){Id=2 ,BookGenre="Nature-Science",BooksInStore="Almanahul unui tinut de nisip" },
            new BooksDisplayed(){Id=3 ,BookGenre="Nature-Science",BooksInStore="H is for Hawk" }
            };
        }

        public BooksDisplayed AddBook(BooksDisplayed books)
        {
            throw new NotImplementedException();
        }

        public BooksDisplayed DeleteBook(int? Id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<BooksDisplayed> GetBooks()
        {
            return _getBooks;
        }

        public BooksDisplayed GetSpecificBook(int? Id)
        {
            throw new NotImplementedException();
        }

        public BooksDisplayed UpdateBook(BooksDisplayed bookUpdate)
        {
            throw new NotImplementedException();
        }
    }
}
