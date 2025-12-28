using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Quiz.DTO;
using Quiz.Models;

namespace Quiz.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IConfiguration config;

        public AccountController(UserManager<ApplicationUser> userManager, IConfiguration config)
        {
            this.userManager = userManager;
            this.config = config;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDTO userFromConsumer)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = new ApplicationUser()
                {
                    UserName = userFromConsumer.UserName,
                    Email = userFromConsumer.Email
                };

                IdentityResult result = await userManager.CreateAsync(user, userFromConsumer.Password);

                if (result.Succeeded)
                {
                    return Ok(new { message = "Account created successfully", success = true });
                }

                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
            }

            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
            return BadRequest(new { message = "Registration failed", success = false, errors = errors });
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO userFromConsumer)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = await userManager.FindByNameAsync(userFromConsumer.UserName);
                if (user != null)
                {
                    bool found = await userManager.CheckPasswordAsync(user, userFromConsumer.Password);
                    if (found)
                    {
                       
                        string jti = Guid.NewGuid().ToString();
                        var userRoles = await userManager.GetRolesAsync(user);


                        List<Claim> claim = new List<Claim>();
                        claim.Add(new Claim(ClaimTypes.NameIdentifier, user.Id));
                        claim.Add(new Claim(ClaimTypes.Name, user.UserName));
            
                        if (userRoles != null)
                        {
                            foreach (var role in userRoles)
                            {
                                claim.Add(new Claim(ClaimTypes.Role, role));
                            }
                        }
                        SymmetricSecurityKey signinKey =
                            new(Encoding.UTF8.GetBytes(config["JWT:Key"]));

                        SigningCredentials signingCredentials =
                            new SigningCredentials(signinKey, SecurityAlgorithms.HmacSha256);

                        JwtSecurityToken myToken = new JwtSecurityToken(
                            issuer: config["JWT:Iss"],
                            audience: config["JWT:Aud"],
                            expires: DateTime.Now.AddDays(1),
                            claims: claim,
                            signingCredentials: signingCredentials
                            );

                        return Ok(new
                        {
                            success = true,
                            message = "Login successful",
                            expired = DateTime.Now.AddDays(1),
                            token = new JwtSecurityTokenHandler().WriteToken(myToken)
                        });
                     
                    }
                }
                ModelState.AddModelError("", "Invalid Account");
            }
            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
            return BadRequest(new
            {
                success = false,
                message = "Login failed",
                errors = errors
            });
        }
    }
}
