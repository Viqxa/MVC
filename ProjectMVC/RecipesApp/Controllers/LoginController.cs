using RecipesApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using LoginRequest = RecipesApp.Models.LoginRequest;

namespace RecipesApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private IConfiguration _configuration;

        public LoginController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost]

        public IActionResult Login([FromBody] LoginRequest loginRequest)
        {
            //Autentykacja użytkownnika

            //if (loginRequest.Email.Contains("aaa"))

            var users = JsonSerializer.Deserialize<List<User>>(
                System.IO.File.ReadAllText("Users.json")) ?? new List<User>();

            var user = users.Find(u => u.Email == loginRequest.Email && u.Password == loginRequest.Password);
            if (user != null)


            {
                var claims = new[] { new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()) };

                var securityKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                var creditionals = new SigningCredentials(
                    securityKey, SecurityAlgorithms.HmacSha256);

                var stoken = new JwtSecurityToken(
                    _configuration["Jwt:Issuer"], _configuration["Jwt:Issuer"],
                    null, expires: DateTime.Now.AddMinutes(15),
                    signingCredentials:creditionals);
                var token = new JwtSecurityTokenHandler().WriteToken(stoken);
                return Ok(token);
            }
            else
                return BadRequest("Błędny login lub haslo!");
        }
    }
}
