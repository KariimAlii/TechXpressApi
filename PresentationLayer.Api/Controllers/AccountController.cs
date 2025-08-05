using DataAccessLayer.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PresentationLayer.Api.ActionRequests;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PresentationLayer.Api.Controllers
{
    [Route("api/[controller]")]  //           /api/account
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _config;

        public AccountController(UserManager<User> userManager, IConfiguration config)
        {
            _userManager = userManager;
            _config = config;
        }
        //   /register  ==> Create a new User
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterUserActionRequest request)
        {
            var user = new User
            {
                UserName = request.UserName,
                Email = request.Email,
                Address = request.Address,
            };

            IdentityResult result = await _userManager.CreateAsync(user, request.Password);

            if(result.Succeeded)
            {
                return Ok("Account Registered");
            }

            var errors = result.Errors.Select(error => error.Description).ToArray();

            return BadRequest(errors);
        }

        //  /login  ==>  Check Credentials ==> if valid ==> create jwt token
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginUserActionRequest request)
        {
            var user = await _userManager.FindByNameAsync(request.UserName);

            if(user != null)
            {
                var isPasswordValid = await _userManager.CheckPasswordAsync(user, request.Password);

                if(isPasswordValid)
                {
                    // Generate JWT

                    // Payload(Claims) + Algorithm + Secret

                    // (1) Payload ( Claims )

                    var claims = new List<Claim>()
                    {
                        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),   // .Net Claims
                        new Claim(ClaimTypes.Name, user.UserName),
                        new Claim(ClaimTypes.Email, user.Email),
                        new Claim("Address", user.Address),                        // Custom Claims
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),   // JWT Claims
                    };

                    // Add Roles to Claims

                    var roles = await _userManager.GetRolesAsync(user);

                    foreach (var role in roles)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, role));
                    }

                    // Signing Signature = Secret + HMAC Algorithm

                    var secret = _config["Jwt:Secret"];

                    var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));

                    var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                    // Generate JWT Token
                    JwtSecurityToken jwtToken = new JwtSecurityToken
                    (
                        //issuer: "http://localhost:5217",
                        issuer: _config["Jwt:Issuer"],

                        //audience: "http://localhost:4200"
                        audience: _config["Jwt:Audience"],

                        claims: claims,

                        expires: DateTime.Now.AddHours(1),

                        signingCredentials: signingCredentials
                    );

                    return Ok
                    (
                        new
                        {
                            token = new JwtSecurityTokenHandler().WriteToken(jwtToken),  // claims + security key + algorith -----encoding-----> string token
                            expiration = jwtToken.ValidTo
                        }
                    );
                }
            }

            return Unauthorized();
        }
    }
}
