using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Models
{
    public class BooksDisplayed
    {
        //[Column("BookId")]
        [Key]
        public int BookId { get; set; }
        public string BooksInStore { get; set; }
        public string BookGenre { get; set; }
        public int Price { get; set; }
        public int StockOfBooks { get; set; }
    }
}
//To change a Column Name in DataBase
/*
 * I added the attribute [Column("NewName")] above the property that I wanted to change the name
 * Add-Migration
 * Update Database
 * 
 * See Results in DataBase
 * 
 * Then you can change the property name, if you want and delete the attribute [Column("NewName")] from the respective property.
 * Then change in all the parts of the code the old name that was used.
 */
