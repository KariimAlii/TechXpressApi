using DataAccessLayer.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PresentationLayer.ActionRequests.Accouts;
using System.Security.Claims;

namespace PresentationLayer.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterUserActionRequest request)
        {
            if(ModelState.IsValid)
            {
                var user = new User
                {
                    Id = Guid.NewGuid(),
                    UserName = request.UserName,
                    PasswordHash = request.Password,
                    Address = request.Address,
                    Email = request.Email
                };

                // _userManager.CreateAsync(user)
                // ==> this overload doesnot hash the password
                IdentityResult result = await _userManager.CreateAsync(user, request.Password); // this overload hashes password

                if(result.Succeeded) // ==> User saved successfully
                {
                    // Create a cookie ( login )
                    await _signInManager.SignInAsync(user, false);
                    return RedirectToAction("Index", "Product");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(error.Code, error.Description);
                    }
                }
            }

            return View(request);
        }
        [HttpGet]
        public IActionResult Login([FromQuery] string? ReturnUrl)
        {
            ViewBag.ReturnUrl = ReturnUrl;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginActionRequest request, string? ReturnUrl)
        {
            if(ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(request.UserName);

                if(user != null) // user exists in database
                {
                    var isPasswordValid = await _userManager.CheckPasswordAsync(user, request.Password);

                    if(isPasswordValid)
                    {
                        // create a cookie (sign in)
                        await _signInManager.SignInAsync(user, request.RememberMe);
                        return RedirectToAction("Index", "Product");
                    }
                }

                ModelState.AddModelError("Invalid Credentials", "UserName or Password invalid");
            }

            return View(request);
        }
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(Login));
        }
    }
}
