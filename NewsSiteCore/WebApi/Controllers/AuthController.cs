using IdentityData.Identity;
using Interfaces.ResultModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WebApi.Models;
using static Interfaces.Enums.Common;

namespace WebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/Auth")]
    public class AuthController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        //private readonly IEmailSender _emailSender;

        public AuthController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager
            //IEmailSender emailSender
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            //_emailSender = emailSender;
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody]LoginViewModel model)
        {
            Result<string> result;
            if (ModelState.IsValid)
            {
                var signInResult = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
                if (signInResult.Succeeded)
                {
                    result = new Result<string>(await GenerateToken(model.Email));
                }
                else
                {
                    result = new Result<string>(false, ResultType.Error, "Girdiğiniz bilgiler yanlış!");
                }
            }
            else
            {
                result = new Result<string>(false, ResultType.Error, "Lütfen tüm bilgileri eksiksiz girdiğinizden emin olunuz!");
            }

            return Json(result);
        }

        private async Task<string> GenerateToken(string userName)
        {
            var user = await _userManager.FindByEmailAsync(userName);
            var roles = await _userManager.GetRolesAsync(user);
            var someClaims = new Claim[] { new Claim(JwtRegisteredClaimNames.Sub, userName), new Claim(JwtRegisteredClaimNames.Email, userName) };
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(someClaims, "Token");
            // Adding roles code
            // Roles property is string collection but you can modify Select code if it it's not
            claimsIdentity.AddClaims(roles.Select(role => new Claim(ClaimTypes.Role, role)));
            SecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("denemedenemedenemedeneme"));
            var token = new JwtSecurityToken(
                audience: "mysite.com",
                issuer: "mysite.com",
                claims: claimsIdentity.Claims,
                expires: DateTime.Now.AddMinutes(10),
                signingCredentials: new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}