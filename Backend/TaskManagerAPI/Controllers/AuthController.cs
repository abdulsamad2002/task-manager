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
    /// <summary>
    /// Handles user registration and login. Issues JWT tokens on successful authentication.
    /// </summary>
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

        /// <summary>
        /// Registers a new user. The role is always forced to Employee (RoleId = 3),
        /// regardless of any value provided in the request body.
        /// </summary>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] User user)
        {
            try
            {
                const int employeeRoleId = 3;

                var newUser = await _context.RegisterUserAsync(
                    user.FullName,
                    user.Email,
                    user.Password,
                    employeeRoleId
                );

                if (newUser == null)
                    return BadRequest(new { message = "Registration failed. Email may already be in use." });

                return Ok(newUser);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Authenticates a user with email and password.
        /// Returns a JWT token and basic user info on success.
        /// </summary>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest login)
        {
            var user = await _context.ValidateUserAsync(login.Email, login.Password);

            if (user == null)
                return Unauthorized(new { message = "Invalid email or password." });

            var token = GenerateJwtToken(user);

            return Ok(new
            {
                token,
                user = new
                {
                    user.UserId,
                    user.FullName,
                    user.Email,
                    user.RoleId
                }
            });
        }

        // ─── Private ──────────────────────────────────────────────────────────────

        /// <summary>
        /// Generates a signed JWT token containing user identity and role claims.
        /// Token is valid for 8 hours.
        /// </summary>
        private string GenerateJwtToken(User user)
        {
            string roleName = user.RoleId switch
            {
                1 => "Admin",
                2 => "Manager",
                _ => "Employee"
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, roleName),
                new Claim("RoleId", user.RoleId.ToString())
            };

            var token = new JwtSecurityToken(
                issuer:   _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims:   claims,
                expires:  DateTime.UtcNow.AddHours(8),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}