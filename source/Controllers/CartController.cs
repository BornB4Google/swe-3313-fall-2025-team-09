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
    public async Task<ActionResult<CartDto>> GetCart(int userId)
    {
        // Andrew Tressler
        //try to active find cart in db
        var Cart = await _db.Carts
            .Where(c => c.UserId == userId && c.isActive)
            .SingleOrDefaultAsync();
        //if none found, make new one
        if (Cart == null)
        {
            var c = new Cart();
            c.UserId = userId;
            c.isActive = true;
            var user = await _db.Users
                .Where(u => u.UserId == userId).
                SingleOrDefaultAsync();
            c.User = user;
            Cart = c;
            await _db.AddAsync(c);
            await _db.SaveChangesAsync();
        }

        var Dto = ConvertCartToDto(Cart);
        return Ok(Dto);
    }
    
    // POST /api/cart/items
    [HttpPost("items")]
    public async Task<IActionResult> AddItem([FromBody] AddToCartRequest request)
         {
             // Andrew To Do- implement add to cart logic
             
             var Item = await _db.InventoryItems.Where(i => i.ItemId == request.ItemId).SingleOrDefaultAsync();
             Cart cart = await _db.Carts
                 .Where(c => c.CartId == request.CartId).
                 SingleOrDefaultAsync();
             
             if(cart == null) return StatusCode(404, "No cart found with id" + request.CartId);
             
             CartItem cItem = new CartItem();
             cItem.CartId = request.CartId;
             cItem.ItemId = request.ItemId;
             cItem.InventoryItem = Item;
             cart.Items.Add(cItem);
             await _db.SaveChangesAsync();
             return StatusCode(200);
         }
    
    // DELETE /api/cart/items/{id}
    [HttpDelete("items/{id:int}")]
    //id = CartItem id
    public async Task<IActionResult> RemoveItem(int id)
    {
        // Andrew To Do- delete from cart logic
        //TODO - Account for invalid IDs or null variables
        CartItem cItem = await _db.CartItems.Where(cI => cI.ItemId == id).SingleOrDefaultAsync();

        if (cItem == null) return StatusCode(404, "No cartItem found with id" + id);
        
        ///Law of Demeter? more like Suggestion of Demeter
        cItem.Cart.Items.Remove(cItem);
        _db.CartItems.Remove(cItem);
        
        await _db.SaveChangesAsync();
        
        return StatusCode(200, "Removed item from cart.");
    }
    
    //helper methods

    private static CartDto ConvertCartToDto(Cart cart)
    {
        var cartItems = cart.Items.ToList();
        var dto = new CartDto();
        foreach (var ci in cartItems)
        {
            CartItemDto cItemDto = new CartItemDto()
            {
                //law of what now?
                CartItemId = ci.ItemId,
                ItemId = ci.InventoryItem.ItemId,
                Name = ci.InventoryItem.Name,
                Category =  ci.InventoryItem.Category,
                UnitPrice = ci.InventoryItem.Price,
                PrimaryPhotoUrl = ci.InventoryItem.PrimaryPhotoUrl
            };
            dto.Items.Add(cItemDto);
        }
        return dto;
        
    }
    
    
    
    
}