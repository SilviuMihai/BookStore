using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Models
{
    public class OrderBooksViewModels
    {
        public IEnumerable<UserWithBooksDB> UserBooksDB { get; set; }
    }
}
