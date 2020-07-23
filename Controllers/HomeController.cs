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
    public class HomeController : Controller
    {
        private readonly IBookStore _bookStore;
        public HomeController(IBookStore bookStore)
        {
            _bookStore = bookStore;
        }
        [AllowAnonymous]
        [HttpGet]
       public ViewResult Index()
        {
            return View();
        }

        //This View has two functionalities:
        //1. Display All Books
        //2.Search a book, by the name
        [HttpGet]
        [AllowAnonymous]
        public ViewResult DisplayBooks(HomeViewModels model) 
        {
            model.BooksDisplayedInStore = _bookStore.GetBooks();
            model.BooksSearched = _bookStore.SearchBook(model.SearchBook);
            return View(model);
        }
    }
}
