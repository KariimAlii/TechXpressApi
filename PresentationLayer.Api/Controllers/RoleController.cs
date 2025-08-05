using DataAccessLayer.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PresentationLayer.Api.ActionRequests;

namespace PresentationLayer.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Moderator")]
    public class RoleController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;

        public RoleController(UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateRole(CreateRoleActionRequest request)
        {
            if(string.IsNullOrWhiteSpace(request.RoleName))
            {
                return BadRequest("Role Name cannot be empty");
            }

            var isRoleExists = await _roleManager.RoleExistsAsync(request.RoleName);

            if(isRoleExists)
            {
                return BadRequest("Role already exists");
            }

            var role = new Role
            {
                Name = request.RoleName,
            };

            var result = await _roleManager.CreateAsync(role);

            if(result.Succeeded)
            {
                return Created();
            }

            return BadRequest(new
            {
                Message = "Failed to Create a new role",
                errors = result.Errors
            });
        }

        [HttpPost("assign-user-role")]
        public async Task<IActionResult> AssignUserRole(AssignUserRoleActionRequest request)
        {
            var user = await _userManager.FindByNameAsync(request.UserName);

            if (user == null)
                return BadRequest("User Not Found");

            var isRoleExists = await _roleManager.RoleExistsAsync(request.RoleName);

            if (!isRoleExists)
                return BadRequest("Role Not Found");

            var isUserInRole = await _userManager.IsInRoleAsync(user, request.RoleName);

            if(!isUserInRole)
            {
                // Assign the user to the role
                var result = await _userManager.AddToRoleAsync(user, request.RoleName);
                if (result.Succeeded)
                {
                    return Ok("User assigned successfully to Role");
                }

                return BadRequest(new
                {
                    Message = "Failed to assign user to role",
                    errors = result.Errors
                });

            }

            return BadRequest($"User is already in role {request.RoleName}");
        }
    }
}
