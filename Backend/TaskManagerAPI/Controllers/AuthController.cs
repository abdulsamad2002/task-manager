using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TaskManagerAPI.Data;
using TaskManagerAPI.DTOs;
using TaskManagerAPI.Models;

namespace TaskManagerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;

        public AuthController(AppDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] User user)
        {
            try
            {
                // FORCE the RoleId to 3 (Employee) no matter what the user sends
                int defaultRole = 3;

                var newUser = await _context.RegisterUserAsync(
                    user.FullName,
                    user.Email,
                    user.Password,
                    defaultRole // <--- We use our hardcoded '3' here
                );

                if (newUser == null) return BadRequest("Registration failed.");
                return Ok(newUser);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest login)
        {
            var user = await _context.ValidateUserAsync(login.Email, login.Password);

            if (user == null) return Unauthorized("Invalid email or password.");

            var token = GenerateJwtToken(user);

            return Ok(new
            {
                Token = token,
                User = new { user.UserId, user.FullName, user.Email, user.RoleId }
            });
        }

        private string GenerateJwtToken(User user)
        {
            // --- NEW: ROLE TRANSLATION LOGIC ---
            // This maps your database RoleId to the string the Controller expects
            string roleName = user.RoleId switch
            {
                1 => "Admin",
                2 => "Manager",
                3 => "Employee",
                _ => "Employee"
            };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                // IMPORTANT: Use ClaimTypes.Role so the [Authorize] attribute can find it
                new Claim(ClaimTypes.Role, roleName),
                new Claim("RoleId", user.RoleId.ToString()) // Keeping this for frontend use
            };

            var token = new JwtSecurityToken(
                _config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddHours(8),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}