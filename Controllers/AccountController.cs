using BookStore.Models;
using BookStore.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Controllers
{
    public class AccountController:Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;

        public AccountController(UserManager<ApplicationUser> userManager,SignInManager<ApplicationUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        [HttpPost]
        [HttpGet]
        //or [AcceptVerbs("Get",Post)]
        //jquery validate method(scripts) issues an ajax call to this method, and the jquery method expects a Json response
        //this is the reason why is returning a Json response
        //check to see the Remote attribute in RegisterViewModel
        //An Ajax call is an asynchronous request initiated by the browser that does not directly result in a page transition.
        public async Task<IActionResult> IsEmailInUse(string email)
        {
            var user = await userManager.FindByEmailAsync(email);

            if (user == null)
            {
                return Json(true); // to make an ajax call possible, accessing the Email from RegisterUserViewModel with the attribute Remote
                                   // also we need the 3 scripts files, in the same order, that are presented in Layout file (jquery)
                                   // so we need to respond with a Json result
            }
            else 
            {
                return Json($"The Email {email}, is already in use.");
            }
        }
        [AllowAnonymous]
        [HttpGet]
        public IActionResult LogIn()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> LogIn(LoginViewModels loginViewModels)
        {
            if (ModelState.IsValid)
            {
                var result = await signInManager.PasswordSignInAsync(loginViewModels.Email, loginViewModels.Password,
                    isPersistent: loginViewModels.RememberMe, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    return RedirectToAction("index", "home");
                }
                //In case the Login fails
               
                    ModelState.AddModelError(string.Empty, "Invalid Login Attempt !"); // add errors to ModelState, to list the problems regarding the registration
            }
            return View(loginViewModels);
        }

        [HttpPost]
        public async Task<IActionResult> LogOut()
        {
               await signInManager.SignOutAsync();

            return RedirectToAction("index", "home");
        }
        [AllowAnonymous]
        [HttpGet]
        public IActionResult RegisterUser()
        {
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> RegisterUser(RegisterUserViewModel registerUserViewModel)
        {
            if (ModelState.IsValid) // checks the fields, when the user adds all the details (like password,username) also checks if they are empty(thats why we have data annotations added in view models or models)
            {
                //IdentityUser is very configurable and the developer can overwrite some commands.
                var user = new ApplicationUser() { Email = registerUserViewModel.Email, UserName = registerUserViewModel.Email }; // to add the user to the database
                var result = await userManager.CreateAsync(user, registerUserViewModel.Password); // create the user and the password hashed to the database
                if (result.Succeeded)
                {
                    await signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("index", "home"); // if succeeded, returns the user to the index
                } 
                //In case the Registration fails
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description); // add errors to ModelState, to list the problems regarding the registration
                }
            }
            return View(registerUserViewModel);
        }

    }
}
