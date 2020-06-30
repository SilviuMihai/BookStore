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
    [Authorize(Roles = "Admin")]

    //(*)[Authorize(Policy ="AdminRolePolicy")](*)
    public class AdministrationController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<ApplicationUser> userManager;

        public AdministrationController(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
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
        public async Task<IActionResult> EditRole(string id, EditRoleViewModels editRoleViewModels)
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
                        return RedirectToAction("EditRole", new { Id = roleId }); // if there are no remainig users, redirect
                    }
                }
            }
            return RedirectToAction("EditRole", new { Id = roleId });
        }

        [Authorize(Policy = "DeleteRolePolicy")]
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
                    if (userInRole) // if there are users in Role, than post this message
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

            return RedirectToAction("EditUser", "Administration", new { Id = userId });
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

            //To keep all the Claims in this object
            var userDataBaseClaims = await userManager.GetClaimsAsync(user);

            var model = new UserClaimsViewModels()
            {
                UserId = userId
            };
            //ClaimStore contains the List of Claims(Claim type and Claim value)
            foreach (Claim claims in ClaimsStore.AllClaims)
            {
                //populate the UserClaim with the Type from the ClaimStore.AllClaims
                //is taken the "Type" from ClaimStore
                //So we can see the values displayed on the view (I mean the Claims Type)
                UserClaim userClaim = new UserClaim()
                {
                    //ClaimType it is string
                    ClaimType = claims.Type
                };

                //Check in Database if the user has that specific Type(if it has set to true IsSelected)
                //"c" is a Claim that has the Type(string) 
                //userDataBaseClaims contains all the Claims that were gathered "var userDataBaseClaims = await userManager.GetClaimsAsync(user);"
                //comparing if it has that Type.
                //populating IsSelected with True
                //if (userDataBaseClaims.Any(c => c.Type == userClaim.ClaimType))
                // {
                //    userClaim.IsSelected = true;
                //}
                if (userDataBaseClaims.Any(c => c.Type == userClaim.ClaimType && c.Value == "true")) //(added value in post)
                    {
                        userClaim.IsSelected = true;
                    }
                    //Add all the ClaimTypes and IsSelected to the ViewModel(UserClaimsViewModels()), so that can be displayed on the View
                    model.Claims.Add(userClaim);
            }
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> ManageUserClaims(UserClaimsViewModels model)
        {
            var user = await userManager.FindByIdAsync(model.UserId);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with the respective ID:{model.UserId} cannot be found.";
                return View("NotFound");
            }
            //Get all Claims for the user
            var userClaims = await userManager.GetClaimsAsync(user);
            //Remove all Claims for the user
            //We avoid putting more conditions(if's), to test if the user is selected on the respective Claim, or not -
            //this is the reason why all claims are deleted
            var result = await userManager.RemoveClaimsAsync(user, userClaims);

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Cannot remove user existing claims !");
                return View(model);
            }

            //Here it checks for what is selected on the view, to add the claims to the user
            //I want to add just only what is selected, this is the reason that I use Where and Select.
            //("Where" returns Ienumarable of UserClaim and we need to return Ienumarable of Claim object(because of AddClaimAsync), this is the reason is put Select function, because return Ienumerable of Claim object)
            //result = await userManager.AddClaimsAsync(user, model.Claims.Where(c => c.IsSelected).Select(c => new Claim(c.ClaimType, c.ClaimType)));


            result = await userManager.AddClaimsAsync(user, model.Claims.Select(c => new Claim(c.ClaimType, c.IsSelected ? "true" : "false")));

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Cannot add selected claim to the user !");
                return View(model);
            }

            return RedirectToAction("EditUser", "Administration", new { Id = model.UserId }); //created an anonymous object because we need to return to the page of EditUser, so it needs the userId
        }

        
        [HttpGet]
        public IActionResult ListUsers()
        {
            var users = userManager.Users;
            return View(users);
        }

        
        [HttpGet]
        public async Task<IActionResult> EditUser(string id)
        {
            var user = await userManager.FindByIdAsync(id);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with the respective ID:{id} cannot be found.";
                return View("NotFound");
            }

            var userGetClaims = await userManager.GetClaimsAsync(user); //get claims from this user
            var userGetRoles = await userManager.GetRolesAsync(user); //get roles from this user

            var editUserModel = new EditUserViewModels()
            {
                Id = user.Id,
                FamilyName = user.SurName,
                Name = user.Name,
                Adress = user.Adress,
                PhoneNumber = user.PhoneNumber,
                City = user.City,
                Country = user.Country,
                Email = user.Email,
                Age = user.Age,
                Roles = userGetRoles,
                Claims = userGetClaims.Select(c => c.Type).ToList()
            };


            return View(editUserModel);
        }

        
        [HttpPost]
        public async Task<IActionResult> EditUser(EditUserViewModels editUserViewModels)
        {
            var user = await userManager.FindByIdAsync(editUserViewModels.Id);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with the respective ID:{editUserViewModels.Id} cannot be found.";
                return View("NotFound");
            }
            else
            {
                if (ModelState.IsValid)
                {
                    user.SurName = editUserViewModels.FamilyName;
                    user.Name = editUserViewModels.Name;
                    user.Adress = editUserViewModels.Adress;
                    user.PhoneNumber = editUserViewModels.PhoneNumber;
                    user.City = editUserViewModels.City;
                    user.Country = editUserViewModels.Country;
                    user.Email = editUserViewModels.Email;
                    user.Age = editUserViewModels.Age;

                    var result = await userManager.UpdateAsync(user);

                    if (result.Succeeded)
                    {
                        return RedirectToAction("ListUsers", "Administration");
                    }
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description); // add errors to ModelStats
                    }
                }
            }
            return View(editUserViewModels);
        }

        
        [HttpPost]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await userManager.FindByIdAsync(id);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with the respective ID:{id} cannot be found.";
                return View("NotFound");
            }

            var deletedUser = await userManager.DeleteAsync(user);

            if (deletedUser.Succeeded)
            {
                return RedirectToAction("ListUsers", "Account");
            }
            foreach (var error in deletedUser.Errors)
            {
                ModelState.AddModelError("", error.Description); // add errors to ModelStats
            }
            return View("ListUsers");
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