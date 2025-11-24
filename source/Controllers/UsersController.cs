using Backend.DTOs;
using Backend.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

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
    [Authorize]
    public async Task<IActionResult> GetCurrentUser()
    {
        var userIdClaim = User.FindFirst("UserId")?.Value;
        if (userIdClaim == null || !int.TryParse(userIdClaim, out var userId))
            return Unauthorized();
        
        var user = await _db.Users
            .Where(u => u.UserId == userId)
            .Select (u => new 
                {
                u.Username,
                u.UserId,
                u.FirstName,
                u.LastName,
                u.Email,
                
                })
            .FirstOrDefaultAsync();
        if (user == null)
            return NotFound();
        return Ok(user);
    }
    
    
    
    
    // PUT /api/users/{id}/role
    // Admin only. change a user to an admin
    [HttpPut("{id:int}/role")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateRole(int id, [FromBody] ChangeUserRoleRequest request)
    {
        //Andrew To Do- implement user to admin status logic
        return StatusCode(501, "Not implemented yet");
    }

}