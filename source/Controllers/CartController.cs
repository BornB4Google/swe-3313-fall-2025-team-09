using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

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
        var sub = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value
                  ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (int.TryParse(sub, out var userId))
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
                Description = i.InventoryItem.Description,
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
        //collect userId from JWT
        var userId = GetUserId();

        var item = await _db.InventoryItems.FindAsync(request.ItemId);

        //validation for null item
        if (item == null)
        {
            return NotFound("Item not found");
        }

        //validation for sold item
        if (item.IsSold)
        {
            return BadRequest("Item is sold out");
        }

        //
        var cart = await _db.Carts
            .Include(c => c.Items)
            .FirstOrDefaultAsync(c => c.UserId == userId && c.isActive);

        if (cart == null)
        {
            cart = new Cart
            {
                UserId = userId,
                isActive = true,
                Items = new List<CartItem>()
            };

            _db.Carts.Add(cart);
            await _db.SaveChangesAsync();
        }


        if (cart.Items.Any(i => i.ItemId == item.ItemId))
        {
            return BadRequest("Item already in cart");
        }


        cart.Items.Add(new CartItem
        {
            CartId = cart.CartId,
            ItemId = request.ItemId,

        });

        await _db.SaveChangesAsync();

        return Ok(new { message = "Added to cart" });
    }



    // DELETE /api/cart/items/{id}
    [HttpDelete("items/{id:int}")]
    public async Task<IActionResult> RemoveItem(int id)
    {
        var userId = GetUserId();

        var cart = await _db.Carts
            .Include(c => c.Items)
            .FirstOrDefaultAsync(c => c.UserId == userId && c.isActive);
        ;

        if (cart == null)
        {
            return NotFound("Cart not found");
        }

        var cartItem = cart.Items.FirstOrDefault(i => i.ItemId == id);

        if (cartItem == null)
        {
            return NotFound("Item not found in cart");
        }

        cart.Items.Remove(cartItem);
        await _db.SaveChangesAsync();

        return Ok(new { message = "Item removed from cart" });
    }

}
