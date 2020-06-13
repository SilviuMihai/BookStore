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
    public class AdministrationController:Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<ApplicationUser> userManager;

        public AdministrationController(RoleManager<IdentityRole> roleManager,UserManager<ApplicationUser> userManager)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
        }

        [HttpGet]
        public IActionResult CreateRole()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole(CreateRoleViewModels createRoleViewModels)
        {
            if (ModelState.IsValid)
            {

                var role = new IdentityRole()
                {
                    Name = createRoleViewModels.RoleName
                };

                var result = await roleManager.CreateAsync(role);

                if (result.Succeeded)
                {
                    return RedirectToAction("ListRoles", "Administration");
                }
                //In case that CreateRole fails
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description); // add errors to ModelState, to list the problems regarding the createrole
                }
            }

            //When the Model State fails , the user can return and add the details again
            return View(createRoleViewModels);
        }

        [HttpGet]
        public IActionResult ListRoles()
        {
            var roles = roleManager.Roles;

            return View(roles);
        }


        [HttpGet]
        public async Task<IActionResult> EditRole(string id)
        {

            var role = await roleManager.FindByIdAsync(id);

            if (role == null)
            {
                //to add a view that says not found or create an ajax
                return RedirectToAction("index", "home");
            }
            
                var model = new EditRoleViewModels()
                {
                    RoleId = role.Id,
                    RoleName = role.Name
                };
            

            foreach (var user in await userManager.GetUsersInRoleAsync(role.Name))
            {
                if (await userManager.IsInRoleAsync(user, role.Name))
                {
                    model.Users.Add(user.Email);
                }
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditRole(string id,EditRoleViewModels editRoleViewModels)
        {
            var role = await roleManager.FindByIdAsync(editRoleViewModels.RoleId);

            if (id != editRoleViewModels.RoleId)
            {
                //to add a view that says not found or create an ajax
                return RedirectToAction("index", "home");
            }
            if (ModelState.IsValid)
            {
                role.Name = editRoleViewModels.RoleName;

                var rolename = await roleManager.UpdateAsync(role);
                if (rolename.Succeeded)
                {
                    return RedirectToAction("ListRoles", "Administration");
                }
                //In case that EditRole fails
                foreach (var error in rolename.Errors)
                {
                    ModelState.AddModelError("", error.Description); // add errors to ModelState, to list the problems regarding the editrole
                }
            }
            return View(editRoleViewModels);
        }

        [HttpGet]
        public async Task<IActionResult> EditUsersInRole(string id)
        {
            ViewBag.roleId = id;
            var role = await roleManager.FindByIdAsync(id);

            if (role == null)
            {
                //trebuie creat un alt view, pentru a specifica ca acel id nu se gaseste
                return RedirectToAction("index", "home");
            }

            var listOfUserInRole = new List<EditUsersInRoleViewModels>();

            foreach (var user in userManager.Users) // so I can parse through the users in database, I used MultipleActiveResultSets=true in appsettings.json
            {
                var userInRole = new EditUsersInRoleViewModels()
                {
                    UserId = user.Id,
                    UserName = user.UserName
                };

                if (await userManager.IsInRoleAsync(user, role.Name))
                {
                    userInRole.IsSelected = true;
                }
                else 
                {
                    userInRole.IsSelected = false;
                }
                listOfUserInRole.Add(userInRole);
            }     
            return View(listOfUserInRole);
        }

        [HttpPost]
        public async Task<IActionResult> EditUsersInRole(List<EditUsersInRoleViewModels> model, string roleId)
        {
            var role = await roleManager.FindByIdAsync(roleId);

            if (role == null)
            {
                //trebuie creat un alt view, pentru a specifica ca acel id nu se gaseste
                return RedirectToAction("index", "home");
            }
            for (int i = 0; i < model.Count; i++)
            {
                var user = await userManager.FindByIdAsync(model[i].UserId);

                IdentityResult identityResult = null;

                if (model[i].IsSelected && !(await userManager.IsInRoleAsync(user, role.Name))) //checks if the user is selected and checks if the user is not already in that specific role
                {
                    identityResult = await userManager.AddToRoleAsync(user, role.Name);//add user for that specific role
                }
                else if (!(model[i].IsSelected) && await userManager.IsInRoleAsync(user, role.Name)) //checks if the user is unselected and if the user is already in that specific role - to remove the user from the role 
                {
                    identityResult = await userManager.RemoveFromRoleAsync(user, role.Name); //remove user from the role
                }
                else 
                {
                    continue;
                }

                if (identityResult.Succeeded)
                {
                    if (i < model.Count - 1) 
                    {
                        continue; // if there are more users, set to continue the for instruction
                    }
                    else
                    {
                        return RedirectToAction("EditRole",new { Id = roleId}); // if there are no remainig users, redirect
                    }
                }
            }
            return RedirectToAction("EditRole", new { Id = roleId });
        }
    }
}
