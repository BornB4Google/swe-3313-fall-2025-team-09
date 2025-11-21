using System.Security.Claims;
using Backend.Data;
using Backend.DTOs;
using Backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CartController : ControllerBase
{
    
    private readonly StorefrontDbContext _db;
    
    public CartController(StorefrontDbContext db)
    {
        _db = db;
    }
    
    // GET /api/cart
    [HttpGet]
    public async Task<IActionResult> GetCart()
    {
        // Andrew to do - get users cart logic

        return StatusCode(501, "Not implemented yet");
    }
    
    // POST /api/cart/items
    [HttpPost("items")]
    public async Task<IActionResult> AddItem([FromBody] AddToCartRequest request)
    {
        // Andrew To Do- implement add to cart logic
        
        return StatusCode(501, "Not implemented yet");
    }
    
    // DELETE /api/cart/items/{id}
    [HttpDelete("items/{id:int}")]
    public async Task<IActionResult> RemoveItem(int id)
    {
        // Andrew To Do- delete from cart logic
        
        return StatusCode(501, "Not implemented yet");
    }
}