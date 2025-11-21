using System.Security.Claims;
using Backend.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize] 
public class OrdersController : ControllerBase
{
    private readonly StorefrontDbContext _db;

    public OrdersController(StorefrontDbContext db)
    {
        _db = db;
    }

    // GET /api/orders
    // Get user's order history
    [HttpGet]
    public async Task<IActionResult> GetOrders()
    {
        // Andrew To Do: get user order history logic
       
        return StatusCode(501, "Not implemented yet");
    }

    // GET /api/orders/{id}
    // Gets order details
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetOrder(int id)
    {
        // Andrew To Do: implement logic to get details on a specific order
        
        return StatusCode(501, "Not implemented yet");
    }

    // POST /api/orders/checkout
    // creates an order from current cart
    [HttpPost("checkout")]
    public async Task<IActionResult> Checkout()
    {
        // Andrew To Do: cart -> order logic
        return StatusCode(501, "Not implemented yet");
    }
}
