using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Backend.DTOs;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController :  ControllerBase
{
    
    // POST /api/auth/register
    [HttpPost("register")]
    [AllowAnonymous]
    public IActionResult Register([FromBody] RegisterRequest request)
    {
        //Andrew To Do: Logic for creating a new user goes here
        return StatusCode(501, "Not implemented yet");
    }
    
    // POST /api/auth/login
    [HttpPost("login")]
    [AllowAnonymous]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        // Andrew To Do!! login logic goes here
        // Isaac.. JWT stuff here
        return StatusCode(501, "Not implemented yet");
    }
    
    // POST /api/auth/logout
    [HttpPost("logout")]
    [AllowAnonymous]
    public IActionResult Logout()
    {
        // Andrew To Do: logout logic
        return StatusCode(501, "Not implemented yet");
    }
}