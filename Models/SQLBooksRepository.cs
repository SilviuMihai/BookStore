using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Models
{
    public class SQLBooksRepository : IBookStore
    {
        private readonly BookStoreTime _book;
        private readonly AppDBContext _context;
        public SQLBooksRepository(AppDBContext context)
        {
            _book = new BookStoreTime() { GetDate = DateTime.Now.ToShortDateString(), GetTime = DateTime.Now.ToShortTimeString() };
            _context = context;
        }
        public IEnumerable<BooksDisplayed> GetBooks()
        {
            return _context.BooksInStore;
        }

        public BookStoreTime GetDateandTime()
        {
            return _book;
        }
    }
}
