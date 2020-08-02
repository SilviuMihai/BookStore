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

        //List all Books
        [HttpGet]
        public IActionResult ListBooks()
        {
            HomeViewModels model = new HomeViewModels();
            model.BooksDisplayedInStore = _bookStore.GetBooks();
            //var model = _bookStore.GetBooks();
            return View(model);
        }

        //Edit a Book
        [HttpGet]
        public IActionResult EditBook(int? id) //It is a short-cut way to write Nullable<int> "?"
        {
            ViewBag.deleteBookId = id; // need the Id from the database, so I can add the option in the EditBook -View to delete the book
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
                Id = book.BookId,
                BooksInStore = book.BooksInStore,
                BookGenre = book.BookGenre,
                Price = book.Price,
                StockOfBooks = book.StockOfBooks
            };
            return View(model);
        }

        //Edit a book (set the changes)
        [HttpPost]
        public IActionResult EditBook(EditBookViewModels model)
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

                _bookStore.UpdateBook(book);
            }
            return View(model);
        }

        //Delete a book
        [HttpPost]
        public IActionResult DeleteBook(int? id)
        {
            //Get the book from the database
            BooksDisplayed book = _bookStore.GetSpecificBook(id);
            //Check for the book
            if (book == null)
            {
                ViewBag.ErrorMessage = $"Book with the respective ID:{id} cannot be found.";
                return View("NotFound");
            }
            _bookStore.DeleteBook(id);
            return RedirectToAction("ListBooks","AdministrationBooks");
        }

        //Add a Book View
        [HttpGet]
        public IActionResult AddBook()
        {
            return View();
        }


        //Add a book, set action
        [HttpPost]
        public IActionResult AddBook(AddBookViewModels model)
        {
            if (ModelState.IsValid)
            {
                BooksDisplayed newBook = new BooksDisplayed()
                {
                    BooksInStore = model.BooksInStore,
                    BookGenre = model.BookGenre,
                    StockOfBooks = model.StockOfBooks,
                    Price = model.Price
                };
                _bookStore.AddBook(newBook);
                return RedirectToAction("ListBooks", "AdministrationBooks");
            }
            return View(model);
        }
    }
}


/*
 * Notes:
 * Use binding to prevent overposting - example :https://docs.microsoft.com/en-us/aspnet/core/tutorials/first-mvc-app/controller-methods-views?view=aspnetcore-3.1
 * 
 * ViewResult and IActionResult - there are big differences between the two functionalities
 */
