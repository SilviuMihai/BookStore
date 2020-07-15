using BookStore.Models;
using BookStore.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdministrationBooksController:Controller
    {
        private readonly IBookStore _bookStore;

        public AdministrationBooksController(IBookStore bookStore)
        {
            _bookStore = bookStore;
        }

        [HttpGet]
        public ViewResult ListBooks()
        {
            HomeViewModels model = new HomeViewModels();
            model.BooksDisplayedInStore = _bookStore.GetBooks();
            return View(model);
        }

        [HttpGet]
        public ViewResult EditBook(int? id) //It is a short-cut way to write Nullable<int> "?"
        {
            if (id == null)
            {
                ViewBag.ErrorMessage = $"Book with the respective ID:{id} cannot be found.";
                return View("NotFound");
            }
            //Get the book from the database
            BooksDisplayed book = _bookStore.GetSpecificBook(id);

            //Check for the book
            if (book == null)
            {
                ViewBag.ErrorMessage = $"Book with the respective ID:{id} cannot be found.";
                return View("NotFound");
            }

            EditBookViewModels model = new EditBookViewModels()
            {
                Id = book.Id,
                BooksInStore = book.BooksInStore,
                BookGenre = book.BookGenre,
                Price = book.Price,
                StockOfBooks = book.StockOfBooks
            };
            return View(model);
        }

        [HttpPost]
        public ViewResult EditBook(EditBookViewModels model)
        {
            //Get the book from the database
            BooksDisplayed book = _bookStore.GetSpecificBook(model.Id);
            //Check for the book
            if (book == null)
            {
                ViewBag.ErrorMessage = $"Book with the respective ID:{model.Id} cannot be found.";
                return View("NotFound");
            }

            if (ModelState.IsValid)
            {
                book.BooksInStore = model.BooksInStore;
                book.BookGenre = model.BookGenre;
                book.StockOfBooks = model.StockOfBooks;
                book.Price = model.Price;
                book = _bookStore.UpdateBook(book);
            }
            return View(model);
            //how to check if the object has been updated in the database ?
        }
    }
}
