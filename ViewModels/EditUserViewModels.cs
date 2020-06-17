using BookStore.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.ViewModels
{
    public class EditUserViewModels
    {
        public EditUserViewModels()
        {
            Claims = new List<string>();
            Roles = new List<string>();
        }

        public string Id { get; set; }
        [Required]
        public string FamilyName { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Adress { get; set; }

        [Required][Phone]
        //[RegularExpression("([1-9][0-9]*)", ErrorMessage = "Count must be a natural number")]
        //to be changed 
        public string PhoneNumber { get; set; }

        public string Books { get; set; }

        [Required]
        public Cities? City { get; set; }

        [Required]
        public Countries? Country { get; set; }

        [Required][EmailAddress]
        public string Email { get; set; }

        [Required]
        public int Age { get; set; }

        public IList<string> Roles { get; set; }
        public List<string> Claims { get; set; }
    }
}
