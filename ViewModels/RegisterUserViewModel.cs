using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.ViewModels
{
    public class RegisterUserViewModel
    {
        [Required]
        [EmailAddress]
        [Remote(action:"IsEmailInUse",controller:"Account")] 
        //here is making an ajax call, using the 3 scripts files
        //this is able to work because of the 3 scripts files extensions that were added in the Layout file.
        //the order how are put, must be respected
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        

        [Display(Name ="Confirm Password")]
        [DataType(DataType.Password)]
        [Compare("Password",ErrorMessage = "Password and Confirmation Password do not match !")]
        public string ConfirmPassword { get; set; }
    }
}
