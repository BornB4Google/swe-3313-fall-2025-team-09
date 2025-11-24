using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Backend.DTOs;
using Backend.Data;
using Backend.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;


namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController :  ControllerBase
{
    private readonly StorefrontDbContext _db;
    
    public AuthController(StorefrontDbContext db)
    {
        _db = db;
    }
    
    // POST /api/auth/register
    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        //Andrew To Do: Logic for creating a new user goes here
        if(string.IsNullOrWhiteSpace(request.Username)) return BadRequest("No username given.");
        if (string.IsNullOrWhiteSpace(request.Password)) return BadRequest("No password given.");
        if (string.IsNullOrWhiteSpace(request.Email)) return BadRequest("No email given.");
        if (await _db.Users.AnyAsync(p => p.Username == request.Username)) return BadRequest("Another user already took this username.");

        User newUser = new User();
        newUser.Username = request.Username;
        String hashedPassword = computePasswordHash(request.Password);
        newUser.PasswordHash = hashedPassword;
        newUser.FirstName = request.FirstName;
        newUser.LastName = request.LastName;
        newUser.Email = request.Email;
        newUser.IsAdmin = false;
        
        _db.Users.Add(newUser);
        await _db.SaveChangesAsync();

        return StatusCode(200, "Successfully registered new account");
    }
    
    // POST /api/auth/login
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        // Andrew To Do!! login logic goes here
        // Isaac.. JWT stuff here
        if(string.IsNullOrWhiteSpace(request.Username)) return BadRequest("No username given.");
        if (string.IsNullOrWhiteSpace(request.Password)) return BadRequest("No password given.");
        
        
        String hashedPassword = computePasswordHash(request.Password);
        
        var user = await _db.Users
            .SingleOrDefaultAsync(u => u.Username == request.Username);
        if (user.PasswordHash == hashedPassword)
        {
            return StatusCode(200, "User successfully logged in.");
        }
        else return BadRequest("Incorrect password");
        
        
        
    }
    
    // POST /api/auth/logout
    [HttpPost("logout")]
    [AllowAnonymous]
    public async Task<IActionResult> Logout()
    {
        // Andrew To Do: logout logic
        return StatusCode(200, "Successfully logged out");
    }

    private String computePasswordHash(String s)
    {
        StringBuilder Sb = new StringBuilder();

        using (var hash = SHA256.Create())            
        {
            Encoding enc = Encoding.UTF8;
            byte[] result = hash.ComputeHash(enc.GetBytes(s));

            foreach (byte b in result)
                Sb.Append(b.ToString("x2"));
        }

        return Sb.ToString();
        
    }
    
}