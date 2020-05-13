using BookStore.ViewModels;
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
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;

        public AccountController(UserManager<IdentityUser> userManager,SignInManager<IdentityUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult LogIn()
        {
            return View();
        }
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

        [HttpGet]
        public IActionResult RegisterUser()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RegisterUser(RegisterUserViewModel registerUserViewModel)
        {
            if (ModelState.IsValid) // checks the fields, when the user adds all the details (like password,username)
            {
                //IdentityUser is very configurable and the developer can overwrite some commands.
                var user = new IdentityUser() { Email = registerUserViewModel.Email, UserName = registerUserViewModel.Email }; // to add the user to the database
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
