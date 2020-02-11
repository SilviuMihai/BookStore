using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Models
{
    public class SQLBooksRepository : IBookStore
    {
        public IEnumerable<BooksDisplayed> GetBooks()
        {
            throw new NotImplementedException();
        }

        public BookStoreTime GetDateandTime()
        {
            throw new NotImplementedException();
        }
    }
}
