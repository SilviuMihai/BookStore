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

        //[HttpGet]
        //public ViewResult EditBook(int? id) //It is a short-cut way to write Nullable<int> "?"
        //{
        //    if (id == null)
        //    {
        //        ViewBag.ErrorMessage = $"Book with the respective ID:{id} cannot be found.";
        //        return View("NotFound");
        //    }

            
        //}
    }
}
