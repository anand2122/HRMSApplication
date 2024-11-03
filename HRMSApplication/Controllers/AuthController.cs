using HRMSApplication.Core.Models;
using HRMSApplication.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace HRMSApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly IConfiguration _configuration; 
        public AuthController(IConfiguration configuration) 
        { 
            _configuration = configuration; 
        }
        
        [HttpPost("login")] 
        public IActionResult Login([FromBody] LoginModel login) 
        { 
            if (ValidateUser(login)) 
            { 
                var key = _configuration["Jwt:Key"]; 
                var issuer = _configuration["Jwt:Issuer"];
                var audience = _configuration["Jwt:Audience"]; 
                var token = JwtTokenHelper.GenerateToken(login.Username, key, issuer, audience); 
                return Ok(new { token });
            } return Unauthorized(); 
        }
        private bool ValidateUser(LoginModel login)
        { 
          //  Validate the user credentials 
                return login.Username == "test" && login.Password == "password"; }
        }
}
