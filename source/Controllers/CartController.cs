using System.Security.Claims;
using Backend.Data;
using Backend.DTOs;
using Backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.Hosting;
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
    
    // GET /api/cart
    [HttpGet]
    public async Task<Cart> GetCart(int userID)
    {
        // Andrew to do - get users cart logic
        var Cart = await _db.Carts
            .Where(c => c.UserId == userID && c.isActive)
            .SingleOrDefaultAsync();
        if (Cart == null)
        {
            Cart c = new Cart();
            c.UserId = userID;
            c.isActive = true;
            User user = await _db.Users
                .Where(u => u.UserId == userID).
                SingleOrDefaultAsync();
            c.User = user;
            await _db.AddAsync(c);
            await _db.SaveChangesAsync();
        }
        return Cart;
    }
    
    // POST /api/cart/items
    [HttpPost("items")]
    public async Task<IActionResult> AddItem([FromBody] AddToCartRequest request)
         {
             // Andrew To Do- implement add to cart logic
             
             var Item = await _db.InventoryItems.Where(i => i.ItemId == request.ItemId).SingleOrDefaultAsync();
             CartItem cItem = new CartItem();
             cItem.CartId = request.CartId;
             cItem.ItemId = request.ItemId;
             cItem.InventoryItem = Item;
             Cart cart = await _db.Carts
                 .Where(c => c.CartId == request.CartId).
                 SingleOrDefaultAsync();
             cart.Items.Add(cItem);
             await _db.SaveChangesAsync();
             return StatusCode(200);
         }
    
    // DELETE /api/cart/items/{id}
    [HttpDelete("items/{id:int}")]
    public async Task<IActionResult> RemoveItem(int id)
    {
        // Andrew To Do- delete from cart logic
        
        
        
        
        return StatusCode(501, "Not implemented yet");
    }
}