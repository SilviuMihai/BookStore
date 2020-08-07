using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.ViewModels
{
    public class EditOrderBooksUserViewModels
    {
        public string UserBooksId { get; set; }
        public string UserId { get; set; }
        public int BookId { get; set; }
        public string BookToBuy { get; set; }
        public int NrBooksOrdered { get; set; }
    }
}
