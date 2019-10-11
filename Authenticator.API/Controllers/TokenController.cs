using Authenticator.API.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Authenticator.API.Controllers
{
    [Route("api/[controller]")]
    public class TokenController : Controller
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;        

        private readonly IConfiguration _configuration;

        public TokenController(IConfiguration configuration, 
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager )
        {
            _configuration = configuration;
            _userManager = userManager;
            _signInManager = signInManager;


        }

        /// <summary>
        /// Metodo não está otimizado ... apenas um teste.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// 

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> RequestToken([FromBody] User request)
        {
            var signmanager = _signInManager.PasswordSignInAsync(request.Name, request.Password, false, false);
            
            //Fake change for identity 
            //if (request.Name == "eduardo" && request.Password == "123")
            if(signmanager.Result.Succeeded)
            {

                var user = await _userManager.FindByNameAsync(request.Name);

                var claims = new[]
                {
                    new Claim(ClaimTypes.Name, request.Name),
                    new Claim(ClaimTypes.MobilePhone, user.Telefone),
                    new Claim(ClaimTypes.Email, user.Email)
                };


                //This part is the wrong place. I'll change for the new class. New component maybe
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["SecurityKey"]));

                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var timeforvalidationminute = Convert.ToDouble(_configuration["TimeForValidationMinute"]);

                var token = new JwtSecurityToken(
                    issuer: _configuration["issuer"],
                    audience: _configuration["audience"],
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(timeforvalidationminute),
                    signingCredentials: creds);

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    ExpiresIn = (long)TimeSpan.FromMinutes(timeforvalidationminute).TotalSeconds

                });
            }

            return BadRequest("Credenciais inválidas!");
        }
    }
}