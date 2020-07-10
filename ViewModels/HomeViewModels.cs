using BookStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.ViewModels
{
    public class HomeViewModels
    {
        public IEnumerable<BooksDisplayed> BooksDisplayedInStore { get; set; }
    }
}
