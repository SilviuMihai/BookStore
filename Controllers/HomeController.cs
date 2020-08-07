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
        public async Task<IActionResult> DisplayBooks(HomeViewModels model) 
        {
            model.BooksDisplayedInStore = _bookStore.GetBooks(); //display books
            model.BooksSearched = _bookStore.SearchBook(model.SearchBook); //search a book

            if (signInManager.IsSignedIn(User))
            {
                var user = await userManager.GetUserAsync(HttpContext.User);
                ViewBag.userId = user.Id;
            }

            return View(model);
        }

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
                    NrBooksOrdered = model.BooksOrdered,
                    TotalPriceBooks = model.BooksOrdered * model.Price
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


        [HttpGet]
        public async Task<IActionResult> OrderBooks(string id)
        {
            var user = await userManager.FindByIdAsync(id);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with the respective ID:{id} cannot be found.";
                return View("NotFound");
            }

            OrderBooksViewModels orderedBooks = new OrderBooksViewModels()
            {
                UserBooksDB = _bookStore.GetUserSpecificBooks(user.Id)
            };

            return View(orderedBooks);
        }

        [HttpGet]
        public IActionResult EditOrder(string id)
        {
            if (id == null)
            {
                ViewBag.ErrorMessage = $"Book with the respective ID:{id} cannot be found.";
                return View("NotFound");
            }
            UserWithBooksDB editOrder = _bookStore.GetUserBookId(id);

            EditOrderBooksUserViewModels model = new EditOrderBooksUserViewModels()
            {
                UserBooksId= editOrder.UserBooksId,
                BookId=editOrder.BookId,
                UserId=editOrder.UserId,
                NrBooksOrdered=editOrder.NrBooksOrdered,
                BookToBuy= editOrder.BookToBuy
            };
               
            return View(model);
        }

        [HttpPost]
        public IActionResult EditOrder(EditOrderBooksUserViewModels model)
        {
            if (model == null)
            {
                ViewBag.ErrorMessage = $"Something has gone wrong.";
                return View("NotFound");
            }
            BooksDisplayed books = _bookStore.GetSpecificBook(model.BookId);

            if (books == null)
            {
                ViewBag.ErrorMessage = $"Something has gone wrong.";
                return View("NotFound");
            }

            UserWithBooksDB userBooks = _bookStore.GetUserBookId(model.UserBooksId);

            if (userBooks == null)
            {
                ViewBag.ErrorMessage = $"Something has gone wrong.";
                return View("NotFound");
            }

            if (userBooks.NrBooksOrdered < model.NrBooksOrdered)
            {
                var x = model.NrBooksOrdered - userBooks.NrBooksOrdered;
                books.StockOfBooks = books.StockOfBooks - x;
                userBooks.NrBooksOrdered = model.NrBooksOrdered;
                _bookStore.UpdateBook(books);
                _bookStore.UpdateUserBook(userBooks);
            }
            else 
            {
                books.StockOfBooks = userBooks.NrBooksOrdered - model.NrBooksOrdered;
                userBooks.NrBooksOrdered = model.NrBooksOrdered;
                _bookStore.UpdateBook(books);
                _bookStore.UpdateUserBook(userBooks);
            }
            return RedirectToAction("OrderBooks", "Home", new { id = userBooks.UserId });
        }


        [HttpPost]
        public IActionResult DeleteBookOrdered(string id)
        {
            if (signInManager.IsSignedIn(User))
            {
                //Get the book from the database
                UserWithBooksDB userBook = _bookStore.GetUserBookId(id);
                //Check for the book
                if (userBook == null)
                {
                    ViewBag.ErrorMessage = $"User Book with the respective ID:{id} cannot be found.";
                    return View("NotFound");
                }
                BooksDisplayed bookUpdate = _bookStore.GetSpecificBook(userBook.BookId);
                bookUpdate.StockOfBooks = bookUpdate.StockOfBooks + userBook.NrBooksOrdered;
                _bookStore.UpdateBook(bookUpdate);
                _bookStore.DeleteUserBookOrder(id);
                return RedirectToAction("OrderBooks", "Home",new { id=userBook.UserId});
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> FinalCommand()
        {
            var user = await userManager.GetUserAsync(HttpContext.User);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with the respective ID:{user.Id} cannot be found.";
                return View("NotFound");
            }
            if (string.IsNullOrEmpty(user.SurName) || string.IsNullOrEmpty(user.Name) || string.IsNullOrEmpty(user.PhoneNumber) ||
                   string.IsNullOrEmpty(user.Adress) || ((user.Age == 0) || (user.City == 0) || (user.Country == 0)))
            {
                return RedirectToAction("CaptureUserDetails", "UserDetails");
            }
            OrderBooksViewModels orderedBooks = new OrderBooksViewModels()
            {
                UserBooksDB = _bookStore.GetUserSpecificBooks(user.Id)
            };

            return View(orderedBooks);
        }
    }
}

/*
 * Note:
 *  Primary keys are unique values to identify rows in a table and cannot appear more than once.
 */
