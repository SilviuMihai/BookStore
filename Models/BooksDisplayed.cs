﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Models
{
    public class BooksDisplayed
    {
        public int Id { get; set; }
        public string BooksInStore { get; set; }
        public string BookGenre { get; set; }
        public int Price { get; set; }
        public int StockOfBooks { get; set; }
    }
}
