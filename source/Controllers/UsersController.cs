using Backend.DTOs;
using Backend.Data;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

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


    //GET /api/users/me
    // returns current logged-n user's profile
    [HttpGet("me")]
    [Authorize(Roles = "User, Admin")]
    public async Task<IActionResult> GetCurrentUser()
    {

        var userId = GetUserId();

        var user = await _db.Users
            .Where(u => u.UserId == userId)
            .Select(u => new
            {
                u.Username,
                u.UserId,
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
    //Admin only - list all users for management
    [HttpGet()]
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
    // Admin only. change a user to an admin
    [HttpPut("{id:int}/role")]
    // [Authorize(Roles = "Admin")] //TODO uncomment whern auth implemented
    public async Task<IActionResult> UpdateRole(int id, [FromBody] AdminDto request)
    {
        var currentUser = GetUserId();

        if (currentUser == id)
        {
            return BadRequest(new { message = "You cannot change your own admin status" });
        }

        var user = await _db.Users.FindAsync(id);


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


    private int GetUserId()
    {
        var sub = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value
                  ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(sub) || !int.TryParse(sub, out var userId))
        {
            throw new UnauthorizedAccessException("Invalid credentials");
        }

        return userId;
    }

}
