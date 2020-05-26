using BookStore.Models;
using BookStore.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Controllers
{
    public class UserDetailsController:Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        public UserDetailsController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult CaptureUserDetails()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> ShowUserDetails()
        {
            //check if has the user values entered all the fields, if not redirect to CaptureDetails
            //must return the values from the database in here
            //example homecontroller
            var userData = await userManager.GetUserAsync(HttpContext.User);
            if (string.IsNullOrEmpty(userData.FullName) || string.IsNullOrEmpty(userData.PhoneNumber) ||
                string.IsNullOrEmpty(userData.Adress) || ((userData.Age == 0) || (userData.City == 0) || (userData.Country == 0)))
            {
                return RedirectToAction("CaptureUserDetails", "UserDetails");
            }

                UserDetailsViewModels userDetailsViewModels = new UserDetailsViewModels()
                {
                    PhoneNumber = userData.PhoneNumber,
                    Books = userData.BooksBought,
                    City = userData.City,
                    Country = userData.Country,
                    Email = userData.Email,
                    Age = userData.Age,
                    FamilyName = userData.FullName,
                    Name = userData.FullName,
                    Adress = userData.Adress
                };

            return View(userDetailsViewModels);
        }


        //enters the user details in the fields
        [HttpPost]
        public async Task<IActionResult> CaptureUserDetails(UserDetailsViewModels userDetailsViewModels)
        {
            if (ModelState.IsValid)
            {

                var addUserDetails = await userManager.GetUserAsync(HttpContext.User);                                                                               
                                                                             
                if (addUserDetails == null)
                {

                    ViewBag.ErrorMessage = $"User with Id = {addUserDetails} cannot be found";
                    return RedirectToAction("Index", "Home");
                    
                }
                        addUserDetails.PhoneNumber = userDetailsViewModels.PhoneNumber;
                        addUserDetails.FullName = userDetailsViewModels.FamilyName + " " + userDetailsViewModels.Name;
                        addUserDetails.BooksBought = userDetailsViewModels.Books;
                        addUserDetails.Adress = userDetailsViewModels.Adress;
                        addUserDetails.City = userDetailsViewModels.City;
                        addUserDetails.Country = userDetailsViewModels.Country;
                        addUserDetails.Age = userDetailsViewModels.Age;
                

                var user = await userManager.UpdateAsync(addUserDetails);

                if (user.Succeeded)
                {
                    return RedirectToAction("ShowUserDetails", "UserDetails");
                }

                foreach (var error in user.Errors)
                {
                    ModelState.AddModelError("", error.Description); // add errors to ModelStats
                }
            }
            return View(userDetailsViewModels);
        }

    }
}
