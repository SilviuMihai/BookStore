using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.ViewModels
{
    public class AddBookViewModels
    {
        [Required]
        public string BooksInStore { get; set; }
        [Required]
        public string BookGenre { get; set; }
        [Required]
        public int Price { get; set; }
        [Required]
        public int StockOfBooks { get; set; }
    }
}
