﻿using BookStore.Models;
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
        public IActionResult ShowUserDetails()
        {
            //check if has the user values entered all the fields, if not redirect to CaptureDetails
            //must return the values from the database in here
            //example homecontroller
            var userData = new ApplicationUser();
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
                var addUserDetails = new ApplicationUser()
                {
                   PhoneNumber = userDetailsViewModels.PhoneNumber,
                   FullName = userDetailsViewModels.FamilyName +" "+ userDetailsViewModels.Name,
                   BooksBought = userDetailsViewModels.Books,
                   Adress = userDetailsViewModels.Adress,
                   City = userDetailsViewModels.City,
                   Country = userDetailsViewModels.Country
                };

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
