using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.ViewModels
{
    public class BookToBuyViewModels
    {
        [Required]
        public int BooksOrdered { get; set; }
        public string UserId { get; set; }
        public int BookId { get; set; }
        public string BooksToBuy { get; set; }
        public string Email { get; set; }
        public int Price { get; set; }                      
        public string BookGenre { get; set; }
        public int StockOfBook { get; set; }
    }
}
