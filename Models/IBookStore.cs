using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Models
{
  public  interface IBookStore
    {
        BookStoreTime GetDateandTime();
        IEnumerable<BooksDisplayed>GetBooks();
    }
}
