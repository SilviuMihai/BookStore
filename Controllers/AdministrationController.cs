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
    }
}
