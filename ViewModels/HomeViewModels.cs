using BookStore.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.ViewModels
{
    public class HomeViewModels
    {
        public IEnumerable<BooksDisplayed> BooksDisplayedInStore { get; set; }
        public IEnumerable<BooksDisplayed> BooksSearched { get; set; }
        public string SearchBook { get; set; }
    }
}
