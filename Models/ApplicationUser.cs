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
        public string City { get; set; }

        public string Adress { get; set; }

        public string Country { get; set; }

        public string PersonName { get; set; }
    }
}
