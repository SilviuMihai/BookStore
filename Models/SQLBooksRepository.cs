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

        public BooksDisplayed AddBook(BooksDisplayed book)
        {
            _context.BooksInStore.Add(book);
            _context.SaveChanges();
            return book;
        }

        public BooksDisplayed DeleteBook(int? Id)
        {
            BooksDisplayed book = _context.BooksInStore.Find(Id);
            if (book != null)
            {
                _context.BooksInStore.Remove(book);
                _context.SaveChanges();
            }
            return book;
        }

        public IEnumerable<BooksDisplayed> GetBooks()
        {
            return _context.BooksInStore;
        }

        public BooksDisplayed GetSpecificBook(int? Id)
        {
            return _context.BooksInStore.Find(Id);
        }

        public BooksDisplayed UpdateBook(BooksDisplayed bookUpdate)
        {
            var book = _context.BooksInStore.Attach(bookUpdate);
            book.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();
            return bookUpdate;
        }

        public IEnumerable<BooksDisplayed> SearchBook(string searchBook)
        {
            var books = from b in _context.BooksInStore select b;
            if (!String.IsNullOrEmpty(searchBook))
            {
                books = books.Where(s => s.BooksInStore.Contains(searchBook));
            }

            return books;
        }
        public UserWithBooksDB AddBookToUser(UserWithBooksDB userWithBooksDB)
        {
            _context.UserBooksConnectionDB.Add(userWithBooksDB);
            _context.SaveChanges();
            return userWithBooksDB;
        }

        public IEnumerable<UserWithBooksDB> GetUserSpecificBooks(string id)
        {
            IQueryable<UserWithBooksDB> userBooks = null;

            if (!String.IsNullOrEmpty(id))
            {
                userBooks = _context.UserBooksConnectionDB.Where(s => s.UserId == id);
            }

            var booksFromDb = userBooks.AsEnumerable();

            return booksFromDb;
        }

        public UserWithBooksDB DeleteUserBookOrder(string Id)
        {
            UserWithBooksDB book = _context.UserBooksConnectionDB.Find(Id);
            if (book != null)
            {
                _context.UserBooksConnectionDB.Remove(book);
                _context.SaveChanges();
            }
            return book;
        }

        public UserWithBooksDB GetUserBookId(string Id)
        {
            return _context.UserBooksConnectionDB.Find(Id);
        }

        //public UserWithBooksDB UpdateUserBook(string id)
        //{
        //    var userBook = _context.UserBooksConnectionDB.Attach(bookUpdate);
        //    book.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
        //    _context.SaveChanges();
        //    return bookUpdate;
        //}
        public UserWithBooksDB UpdateUserBook(string id)
        {
            throw new NotImplementedException();
        }


    }
}
