using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

using Backend.DTOs;
using Backend.Data;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly StorefrontDbContext _db;

    public UsersController(StorefrontDbContext db)
    {
        _db = db;
    }

    // GET /api/users/me
    // Returns the currently logged-in user's profile based on JWT "sub" claim
    [HttpGet("me")]
    [Authorize]
    public async Task<IActionResult> GetCurrentUser()
    {
        // JWT is created in AuthController with:
        var sub = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value
                  ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (sub == null || !int.TryParse(sub, out var userId))
            return Unauthorized();

        var user = await _db.Users
            .Where(u => u.UserId == userId)
            .Select(u => new
            {
                u.UserId,
                u.Username,
                u.FirstName,
                u.LastName,
                u.Email,
                u.IsAdmin
            })
            .FirstOrDefaultAsync();

        if (user == null)
            return NotFound();

        return Ok(user);
    }

    // GET /api/users
    // Admin only - list all users for management
    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAll()
    {
        var users = await _db.Users
            .OrderBy(u => u.UserId)
            .Select(u => new
            {
                u.UserId,
                u.Username,
                u.FirstName,
                u.LastName,
                u.Email,
                u.IsAdmin
            })
            .ToListAsync();

        return Ok(users);
    }

    // GET /api/users/{id}
    // Admin only - get specific user details
    [HttpGet("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetById(int id)
    {
        var user = await _db.Users
            .Where(u => u.UserId == id)
            .Select(u => new
            {
                u.UserId,
                u.Username,
                u.FirstName,
                u.LastName,
                u.Email,
                u.IsAdmin
            })
            .FirstOrDefaultAsync();

        if (user == null)
            return NotFound();

        return Ok(user);
    }

    // PUT /api/users/{id}/role
    // Admin only - change a user to/from admin
    [HttpPut("{id:int}/role")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateRole(int id, [FromBody] AdminDto request)
    {
        if (request == null)
            return BadRequest("Request body is required.");

        var user = await _db.Users.FindAsync(id);
        if (user is null)
            return NotFound(new { message = "User not found" });

        user.IsAdmin = request.IsAdmin;
        await _db.SaveChangesAsync();

        return Ok(new
        {
            message = "User role updated successfully",
            userId = user.UserId,
            username = user.Username,
            isAdmin = user.IsAdmin
        });
    }
}


