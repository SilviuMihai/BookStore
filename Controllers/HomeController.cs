using BookStore.Models;
using BookStore.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Controllers
{
    public class HomeController : Controller
    {
        private readonly IBookStore _bookStore;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly UserManager<ApplicationUser> userManager;

        public HomeController(IBookStore bookStore, SignInManager<ApplicationUser> signInManager,UserManager<ApplicationUser> userManager)
        {
            _bookStore = bookStore;
            this.signInManager = signInManager;
            this.userManager = userManager;
        }
        [AllowAnonymous]
        [HttpGet]
       public ViewResult Index()
        {
            return View();
        }

        //This View has two functionalities:
        //1. Display All Books
        //2. Search a book, by the name
        [HttpGet]
        [AllowAnonymous]
        public IActionResult DisplayBooks(HomeViewModels model) 
        {
            model.BooksDisplayedInStore = _bookStore.GetBooks(); //display books
            model.BooksSearched = _bookStore.SearchBook(model.SearchBook); //search a book
            return View(model);
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> AddBookInBag(int? id)
        {
            if (signInManager.IsSignedIn(User))
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
                var user = await userManager.GetUserAsync(User);
                
                BookToBuyViewModels model = new BookToBuyViewModels()
                {
                    UserId = user.Id,
                    Email = user.Email,
                    BookId = book.BookId,
                    StockOfBook =book.StockOfBooks,
                    BooksToBuy = book.BooksInStore,
                    Price = book.Price,
                    BookGenre = book.BookGenre
                };
                ViewBag.bookId = book.BookId;
                ViewBag.userId = user.Id;
                return View(model);
            }
            return RedirectToAction("LogIn", "Account");
        }
        [HttpPost]
        public IActionResult AddBookInBag(BookToBuyViewModels model)
        {
            //Get the book from database
            BooksDisplayed book = _bookStore.GetSpecificBook(model.BookId);

            //check if the book is null
            if (book == null)
            {
                ViewBag.ErrorMessage = $"Book with the respective ID:{model.BookId} cannot be found.";
                return View("NotFound");
            }

            if (ModelState.IsValid)
            {
                UserWithBooksDB userBooks = new UserWithBooksDB()
                {
                    UserId = model.UserId,
                    BookId = model.BookId,
                    BookToBuy = model.BooksToBuy,
                    NrBooksOrdered = model.BooksOrdered
                };
                if (model.BooksOrdered < book.StockOfBooks)
                {
                    book.StockOfBooks = book.StockOfBooks - model.BooksOrdered;                    
                    _bookStore.UpdateBook(book);
                    _bookStore.AddBookToUser(userBooks);
                    return RedirectToAction("DisplayBooks", "Home");
                }
                else 
                {
                    return View(model);
                }
            }
            return View(model);
        }
    }
}

/*
 * Note:
 *  Primary keys are unique values to identify rows in a table and cannot appear more than once.
 */
