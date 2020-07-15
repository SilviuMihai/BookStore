using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Models
{
  public  interface IBookStore
    {
        IEnumerable<BooksDisplayed>GetBooks();
        BooksDisplayed GetSpecificBook(int? Id);
        BooksDisplayed AddBook(BooksDisplayed book);
        BooksDisplayed UpdateBook(BooksDisplayed bookUpdate);
        BooksDisplayed DeleteBook(int? Id);

    }
}
