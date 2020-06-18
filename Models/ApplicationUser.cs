using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
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

        public string SurName { get; set; } 

        public string Name { get; set; }

        public string BooksBought { get; set; }

        public int Age { get; set; }
    }
}
//To change a Column Name
/*
 * I added the attribute [Column("NewName")] above the property that I wanted to change the name
 * Add-Migration
 * Update Database
 * See Results
 * Then change in all the parts of the code the old name that was used
 */