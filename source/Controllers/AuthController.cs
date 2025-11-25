using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Backend.DTOs;
using Backend.Data;
using Backend.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;

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
        if (string.IsNullOrWhiteSpace(request.Username)) return BadRequest("No username given.");
        if (string.IsNullOrWhiteSpace(request.Password)) return BadRequest("No password given.");
        if (string.IsNullOrWhiteSpace(request.Email)) return BadRequest("No email given.");
        if (await _db.Users.AnyAsync(p => p.Username == request.Username)) return BadRequest("Another user already took this username.");

        var newUser = new User
        {
            Username = request.Username,
            PasswordHash = computePasswordHash(request.Password),
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            IsAdmin = false
        };

        _db.Users.Add(newUser);
        await _db.SaveChangesAsync();

        return StatusCode(200, "Successfully registered new account");
    }

    // POST /api/auth/login
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Username)) return BadRequest("No username given.");
        if (string.IsNullOrWhiteSpace(request.Password)) return BadRequest("No password given.");

        string hashedPassword = computePasswordHash(request.Password);

        var user = await _db.Users
            .SingleOrDefaultAsync(u => u.Username == request.Username);

        if (user == null)
            return BadRequest("User not found.");

        if (user.PasswordHash != hashedPassword)
            return BadRequest("Incorrect password");

        // ---------- Build JWT token ----------
        var jwtSection = _config.GetSection("Jwt");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSection["Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.UserId.ToString()),
            new Claim(JwtRegisteredClaimNames.UniqueName, user.Username),
            new Claim("isAdmin", user.IsAdmin ? "true" : "false")
        };

        var expiryMinutes = int.TryParse(jwtSection["ExpiryMinutes"], out var mins) ? mins : 60;

        var token = new JwtSecurityToken(
            issuer: jwtSection["Issuer"],
            audience: jwtSection["Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(expiryMinutes),
            signingCredentials: creds
        );

        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

        // ---------- Set JWT as HttpOnly cookie ----------
        Response.Cookies.Append(
            "authToken",
            tokenString,
            new CookieOptions
            {
                HttpOnly = true,
                Secure = true,           // assume HTTPS in prod; in dev you can temporarily set false
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddMinutes(expiryMinutes)
            });

        // OPTIONAL: still return basic info (no token needed in body anymore)
        return Ok(new LoginResponse
        {
            Token = string.Empty,       // not used when using cookie-based auth
            Username = user.Username,
            IsAdmin = user.IsAdmin
        });
    }

    // POST /api/auth/logout
    [HttpPost("logout")]
    [Authorize]  // only logged-in users should log out logically
    public IActionResult Logout()
    {
        // Delete the auth cookie so browser stops sending the token
        Response.Cookies.Delete("authToken");
        return StatusCode(200, "Successfully logged out");
    }

    private string computePasswordHash(string s)
    {
        var sb = new StringBuilder();

        using (var hash = SHA256.Create())
        {
            Encoding enc = Encoding.UTF8;
            byte[] result = hash.ComputeHash(enc.GetBytes(s));

            foreach (byte b in result)
                sb.Append(b.ToString("x2"));
        }

        return sb.ToString();
    }
}
