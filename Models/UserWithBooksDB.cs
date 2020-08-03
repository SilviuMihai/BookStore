using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Models
{
    public class UserWithBooksDB
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]  // to generate an ID(string) automatically
        [Key]
        public string UserBooksId { get; set; }

        [ForeignKey("ApplicationUser")]
        public string UserId { get; set; }

        [ForeignKey("BooksDisplayed")]
        public int BookId { get; set; }
        public string BookToBuy { get; set; }
        public int NrBooksOrdered { get; set; }
    }
}
