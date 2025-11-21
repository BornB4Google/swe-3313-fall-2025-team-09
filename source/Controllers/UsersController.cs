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
    
    // PUT /api/users/{id}/role
    // Admin only.. change a user to an admin
    [HttpPut("{id:int}/role")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateRole(int id, [FromBody] ChangeUserRoleRequest request)
    {
        //Andrew To Do- implement user to admin status logic
        return StatusCode(501, "Not implemented yet");
    }

}