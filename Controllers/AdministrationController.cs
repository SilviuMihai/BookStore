using BookStore.Models;
using BookStore.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BookStore.Controllers
{
    [Authorize(Roles ="Admin")]
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
                ViewBag.ErrorMessage = $"Role with the respective ID:{id} cannot be found.";
                return View("NotFound");
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
                ViewBag.ErrorMessage = $"Role with the respective ID:{roleId} cannot be found.";
                return View("NotFound");
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

        [HttpPost]
        public async Task<IActionResult> DeleteRole(string id)
        {
            bool userInRole = false;
            var role = await roleManager.FindByIdAsync(id);
            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with the respective ID:{id} cannot be found.";
                return View("NotFound");
            }

            // check if there are any users in this role
            foreach (var user in userManager.Users)
            {
                if (await userManager.IsInRoleAsync(user, role.Name))
                {
                    userInRole = true;
                    if(userInRole) // if there are users in Role, than post this message
                    {
                        ViewBag.ErrorTitle = $"{role.Name}  - role, it is used by other users !";
                        ViewBag.ErrorMessage = $"{role.Name}- role, cannot be deleted because it used by other users." +
                            $" If you want to delete the role, remove the users from this role and try again.";
                        return View("Error");
                    }
                }
            }

            var deletedRole = await roleManager.DeleteAsync(role);

            if (deletedRole.Succeeded)
            {
                return RedirectToAction("ListRoles", "Administration");
            }
            foreach (var error in deletedRole.Errors)
            {
                ModelState.AddModelError("", error.Description); // add errors to ModelStats
            }

            return View("ListRoles");
        }

        [HttpGet]
        public async Task<IActionResult> ManageUserRoles(string userId)
        {
            ViewBag.userId = userId;

            var user = await userManager.FindByIdAsync(userId);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with the respective ID:{userId} cannot be found.";
                return View("NotFound");
            }

            var listOfRoles = new List<ManageUserRolesViewModels>();

            foreach (var roles in roleManager.Roles)
            {
                var manageUserRoles = new ManageUserRolesViewModels()
                {
                    RoleId = roles.Id,
                    RoleName = roles.Name
                };

                if (await userManager.IsInRoleAsync(user, roles.Name))
                {
                    manageUserRoles.IsSelected = true;
                }
                else
                {
                    manageUserRoles.IsSelected = false;
                }
                listOfRoles.Add(manageUserRoles);
            }
            return View(listOfRoles);
        }

        [HttpPost]
        public async Task<IActionResult> ManageUserRoles(List<ManageUserRolesViewModels> model, string userId)
        {
            var user = await userManager.FindByIdAsync(userId);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with the respective ID:{userId} cannot be found.";
                return View("NotFound");
            }
            var roles = await userManager.GetRolesAsync(user);
            var result = await userManager.RemoveFromRolesAsync(user, roles);

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Cannot remove user existing roles");
                return View(model);
            }

            result = await userManager.AddToRolesAsync(user, model.Where(x => x.IsSelected).Select(y => y.RoleName));

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Cannot add selected roles to the user");
                return View(model);
            }

            return RedirectToAction("EditUser","Account", new { Id = userId });
        }

        [HttpGet]
        public async Task<IActionResult> ManageUserClaims(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with the respective ID:{userId} cannot be found.";
                return View("NotFound");
            }

            var userDataBaseClaims = await userManager.GetClaimsAsync(user);

            var model = new UserClaimsViewModels()
            {
                UserId = userId
            };
            //ClaimStore contains the List of Claims of type Claims
            foreach (Claim claims in ClaimsStore.AllClaims)
            {
                //populate the UserClaim with the Type from the ClaimStore.AllClaims
                UserClaim userClaim = new UserClaim()
                {
                    ClaimType = claims.Type
                };

                //Check in Database if the user has that specific Type(if it has set to true that Type)
                if (userDataBaseClaims.Any(c => c.Type == userClaim.ClaimType))
                {
                    userClaim.IsSelected = true;
                }
                //Add all the Types to the ViewModel(UserClaimsViewModels()), so that can be displayed on the View
                model.Claims.Add(userClaim);
            }
            return View(model);
        }
    }
}
/*
 * Notes:
 Regarding the tag helpers:
 On the Post operation, we need the id from the database in case that element will be changed, so in the view interface
 with the "model.id" is not possible. It is possible to do that with <input asp-for="id" type="hidden"/>.
 Because in the function of the Post operation, that function has a parameter "(string id)" which expects the id from the View.

 On the Get operation in the case when we have an anchor and we use "asp-route". It is important that the name of asp-route-"id"
 is exactly the same with the name in the function paramater of the Get operation.
 example:
 <a asp-controller="Account" asp-action="EditUser" asp-route-userId="@userId"></a>
 Now in the controller:
 public IActionResult EditUser(string userId)

    As you can see they are both the same.
 */