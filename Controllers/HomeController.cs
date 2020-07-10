﻿using BookStore.Models;
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
        [HttpGet]
        [AllowAnonymous]
        public ViewResult DisplayBooks() 
        {
            HomeViewModels model = new HomeViewModels();
            model.BooksDisplayedInStore = _bookStore.GetBooks();
            return View(model);
        }
  
    }
}
