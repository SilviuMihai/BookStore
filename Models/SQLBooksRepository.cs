using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Models
{
    public class SQLBooksRepository : IBookStore
    {
        private readonly AppDBContext _context;
        public SQLBooksRepository(AppDBContext context)
        {
            _context = context;
        }
        public IEnumerable<BooksDisplayed> GetBooks()
        {
            return _context.BooksInStore;
        }
    }
}
