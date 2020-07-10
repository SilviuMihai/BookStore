using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Models
{
    public class BooksDisplayed
    {
        public string BooksInStore { get; set; }
        public int Id { get; set; }
        public string BookGenre { get; set; }
        public int Price { get; set; }
    }
}
