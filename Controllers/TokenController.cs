using Authenticator.API.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Authenticator.API.Controllers
{
    [Route("api/[controller]")]
    public class TokenController : Controller
    {

        private readonly IConfiguration _configuration;

        public TokenController(IConfiguration configuration)
        {
            _configuration = configuration;

        }

        /// <summary>
        /// Metodo não está otimizado ... apenas um teste.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// 

        [AllowAnonymous]
        [HttpPost]
        public IActionResult RequestToken([FromBody] User request)
        {
            //Fake change for identity 
            if (request.Name == "eduardo" && request.Password == "123")
            {
                var claims = new[]
                {
                    new Claim(ClaimTypes.Name, request.Name)
                };


                //This part is the wrong place. I'll change for the new class. 
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["SecurityKey"]));

                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    issuer: "eduardo.teste",
                    audience: "eduardo.teste",
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(20),
                    signingCredentials: creds);

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token)
                });
            }

            return BadRequest("Credenciais inválidas!");
        }
    }
}