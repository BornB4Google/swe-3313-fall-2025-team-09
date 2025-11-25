using System.Security.Claims;
using Backend.Data;
using Backend.DTOs;
using Backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
    
    
    //Exract userId from JWT token for security
    private int GetUserId()
    {
        if (int.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var userId))
            return userId;
        throw new UnauthorizedAccessException("Invalid user ID");
    }
    
    
    // GET /api/cart
    [HttpGet]
    public async Task<IActionResult> GetCart()
    {
        //collect userId from JWT
        var userId = GetUserId();
        
        var cart = await _db.Carts
            .Include(c => c.Items)
                .ThenInclude(i => i.InventoryItem)
            .FirstOrDefaultAsync(c => c.UserId == userId && c.isActive);


        //if cart is empty, return an empty CartDTO
        if (cart == null)
        {
            return Ok(new CartDto
            {
                Items = new List<CartItemDto>(),
                Total = 0

            });
        }

        //return DTO instead of database entity
        var response = new CartDto
        {
            Items = cart.Items.Select(i => new CartItemDto
            {
                CartItemId = i.ItemId,
                ItemId = i.ItemId,
                Name = i.InventoryItem.Name,
                Category = i.InventoryItem.Category,
                UnitPrice = i.InventoryItem.Price,
                PrimaryPhotoUrl = i.InventoryItem.PrimaryPhotoUrl
            }).ToList(),

            Total = cart.Items.Sum(i => i.InventoryItem.Price)
        };

        return Ok(response); //return the cart
    }
    
    // POST /api/cart/items
    [HttpPost("items")]
    public async Task<IActionResult> AddItem([FromBody] AddToCartRequest request)
    {
        var userId = GetUserId();

        var item = await _db.InventoryItems.FindAsync(request.ItemId);

        if (item == null)
        {
            return NotFound("Item not found");
        }
        
        if (item.IsSold)
    }
    
    // DELETE /api/cart/items/{id}
    [HttpDelete("items/{id:int}")]
    public async Task<IActionResult> RemoveItem(int id)
    {
        // Andrew To Do- delete from cart logic
        
        return StatusCode(501, "Not implemented yet");
    }
}