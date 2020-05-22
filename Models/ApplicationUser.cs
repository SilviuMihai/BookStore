using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Models
{
    //class made for: to add more details in database(User section) ,regarding the user details.
    public class ApplicationUser:IdentityUser 
    {
        public Cities? City { get; set; }

        public string Adress { get; set; }

        public Countries? Country { get; set; }

        public string FullName { get; set; }

        public string BooksBought { get; set; }

        public int Age { get; set; }
    }
}
