using DataAccessLayer.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PresentationLayer.VMs.Admin;

namespace PresentationLayer.Controllers
{
    public class AdminController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;

        public AdminController(UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpGet]
        public IActionResult AssignUserRole()
        {
            var users = _userManager.Users.ToList();
            var roles = _roleManager.Roles.ToList();

            var model = new UserRoleVM
            {
                Users = users.Select(user => new SelectListItem { Text = user.UserName, Value = user.Id.ToString() }).ToList(),
                Roles = roles.Select(role => new SelectListItem { Text = role.Name, Value = role.Id.ToString() }).ToList(),
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AssignUserRole(UserRoleVM request)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(request.UserId.ToString());
                var role = await _roleManager.FindByIdAsync(request.RoleId.ToString());

                if(user == null ||  role == null)    // one of the 2 ids is invalid
                {
                    ModelState.AddModelError("Invalid Data", "Invalid User or Role");
                }

                var result = await _userManager.AddToRoleAsync(user, role.Name);

                if(result.Succeeded)
                {
                    TempData["SuccessMessage"] = "User has been successfully assigned to the role";
                    return RedirectToAction(nameof(AssignUserRole));
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }
            }
            return View(request);
        }

        [HttpGet]
        public async Task<IActionResult> ListUsers()
        {
            var users = _userManager.Users.ToList();
            return View(users);
        }

        [HttpGet]
        public async Task<IActionResult> ListUserRoles()
        {
            var users = _userManager.Users.ToList();

            var userRolesList = new List<UserRolesVM>();

            foreach (var user in users)
            {
                var userRolesVM = new UserRolesVM
                {
                    UserId = user.Id,
                    UserName = user.UserName,
                    Roles = await _userManager.GetRolesAsync(user)
                };
                userRolesList.Add(userRolesVM);
            }

            ViewBag.AllRoles = _roleManager.Roles
                .Select(role => role.Name)
                .ToList();

            return View(userRolesList);
            
        }

        [HttpPost]
        public async Task<IActionResult> EditUserRoles(string userId, List<string> roles)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return NotFound();
            }

            var currentRoles = await _userManager.GetRolesAsync(user);
            var rolesToAdd = roles.Except(currentRoles).ToList();
            var rolesToRemove = currentRoles.Except(roles).ToList();

            await _userManager.AddToRolesAsync(user, rolesToAdd);

            await _userManager.RemoveFromRolesAsync(user, rolesToRemove);

            return RedirectToAction(nameof(ListUserRoles));
            //    current [1,3]
            //    updated [1,4]

            //    to  add      =   updated.Except(current)  ===>  [4]
            //    to  remove   =   current.Except(updated)  ===>  [3]
        }
    }
}
