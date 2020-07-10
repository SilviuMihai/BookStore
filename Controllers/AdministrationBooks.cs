using BookStore.Models;
using BookStore.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Controllers
{
    public class AdministrationBooks:Controller
    {
        private readonly IBookStore _bookStore;

        public AdministrationBooks(IBookStore bookStore)
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
    }
}
