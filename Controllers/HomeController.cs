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
            //var model = _bookStore.GetDateandTime();
            //return View(model);
            HomeViewModels model = new HomeViewModels()
            {
                GetBookStoreTime = _bookStore.GetDateandTime(),
                BooksDisplayedInStore = _bookStore.GetBooks()
            };
            return View(model);
        }
        [HttpGet]
        public ViewResult DisplayBooks() 
        {
            //var model = _bookStore.GetBooks();
            //return View(model);
            HomeViewModels model = new HomeViewModels()
            {
                GetBookStoreTime = _bookStore.GetDateandTime(),
                BooksDisplayedInStore = _bookStore.GetBooks()
            };
            return View(model);
        }
    }
}
