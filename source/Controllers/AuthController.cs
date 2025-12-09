using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

using Backend.Data;
using Backend.DTOs;
using Backend.Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly StorefrontDbContext _db;
    private readonly IConfiguration _config;

    public AuthController(StorefrontDbContext db, IConfiguration config)
    {
        _db = db;
        _config = config;
    }

    // POST /api/auth/register
    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Username))
            return BadRequest("No username given.");
        if (string.IsNullOrWhiteSpace(request.Password))
            return BadRequest("No password given.");
        if (string.IsNullOrWhiteSpace(request.Email))
            return BadRequest("No email given.");

        if (await _db.Users.AnyAsync(u => u.Username == request.Username))
            return BadRequest("Another user already took this username.");
        if (await _db.Users.AnyAsync(u => u.Email == request.Email))
            return BadRequest("An account with this email already exists.");


        var newUser = new User
        {
            Username = request.Username,
            PasswordHash = PasswordHasher.ComputePasswordHash(request.Password),
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            IsAdmin = false
        };

        _db.Users.Add(newUser);
        await _db.SaveChangesAsync();

        return Ok(new { message = "Successfully registered new account" });
    }

    // POST /api/auth/login
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Username))
            return BadRequest("No username given.");
        if (string.IsNullOrWhiteSpace(request.Password))
            return BadRequest("No password given.");
        if (request.Password.Length < 6)
            return BadRequest("Password must be at least 6 characters");

        var user = await _db.Users
            .SingleOrDefaultAsync(u => u.Username == request.Username);

        if (user == null)
            return BadRequest("User not found.");

        var hashedPassword = PasswordHasher.ComputePasswordHash(request.Password);
        if (user.PasswordHash != hashedPassword)
            return BadRequest("Incorrect password");

        // Build JWT toke
        var jwtSection = _config.GetSection("Jwt");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSection["Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            // subject / user id
            new Claim(JwtRegisteredClaimNames.Sub, user.UserId.ToString()),
            // username
            new Claim(JwtRegisteredClaimNames.UniqueName, user.Username),
            // admin flag
            new Claim("isAdmin", user.IsAdmin ? "true" : "false"),
            // user claim
            new Claim(ClaimTypes.Role, "User")
        };

        // add role claim
        if (user.IsAdmin)
        {
            claims.Add(new Claim(ClaimTypes.Role, "Admin"));
        }

        var expiryMinutes = int.TryParse(jwtSection["ExpiryMinutes"], out var mins)
            ? mins
            : 60;

        var token = new JwtSecurityToken(
            issuer: jwtSection["Issuer"],
            audience: jwtSection["Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(expiryMinutes),
            signingCredentials: creds
        );

        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

        // Set JWT cookie
        Response.Cookies.Append(
            "authToken",
            tokenString,
            new CookieOptions
            {
                HttpOnly = true,
                Secure = false,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddMinutes(expiryMinutes)
            });

        return Ok(new LoginResponse
        {
            // token is carried only in cookie for this app
            Token = string.Empty,
            Username = user.Username,
            IsAdmin = user.IsAdmin
        });
    }

    // POST /api/auth/logout
    [HttpPost("logout")]
    [Authorize(Roles = "User, Admin")]
    public IActionResult Logout()
    {
        Response.Cookies.Delete("authToken");
        return Ok(new { message = "Successfully logged out" });
    }

}
