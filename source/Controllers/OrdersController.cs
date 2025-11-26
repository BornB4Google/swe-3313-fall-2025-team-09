using Backend.Data;
using Backend.DTOs;
using Backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize] // all endpoints require auth
public class OrdersController : ControllerBase
{
    private readonly StorefrontDbContext _db;
    private readonly ILogger<OrdersController> _logger;
    private const decimal TAX_RATE = 0.06m;

    public OrdersController(StorefrontDbContext db, ILogger<OrdersController> logger)
    {
        _db = db;
        _logger = logger;
    }

    // GET /api/orders
    // Customer- their orders
    // Admin- all orders
    [HttpGet]
    public async Task<ActionResult<IEnumerable<OrderSummaryDto>>> GetOrders()
    {
        var userId = GetUserIdFromClaims();
        var isAdmin = IsAdminFromClaims();

        IQueryable<Sale> query = _db.Sales
            .AsNoTracking()
            .Include(s => s.Items);

        if (!isAdmin)
        {
            query = query.Where(s => s.UserId == userId);
        }

        var sales = await query
            .OrderByDescending(s => s.SaleDateTime)
            .ToListAsync();

        var result = sales.Select(s => new OrderSummaryDto
        {
            SaleId = s.SaleId,
            SaleDateTime = s.SaleDateTime,
            Total = s.Total,
            ItemCount = s.Items.Count
        }).ToList();

        return Ok(result);
    }

    // GET /api/orders/{id}
    // Customer- can only see their own
    // Admin- can see any
    [HttpGet("{id:int}")]
    public async Task<ActionResult<OrderDetailDto>> GetOrder(int id)
    {
        var userId = GetUserIdFromClaims();
        var isAdmin = IsAdminFromClaims();

        var sale = await _db.Sales
            .AsNoTracking()
            .Include(s => s.User)
            .Include(s => s.Items)
                .ThenInclude(si => si.InventoryItem)
            .SingleOrDefaultAsync(s => s.SaleId == id);

        if (sale == null)
        {
            return NotFound();
        }

        if (!isAdmin && sale.UserId != userId)
        {
            return Forbid();
        }

        var dto = MapToDetailDto(sale);
        return Ok(dto);
    }

 // POST /api/orders/checkout
// Turn the current user's active cart into a Sale
[HttpPost("checkout")]
public async Task<ActionResult<OrderDetailDto>> Checkout([FromBody] CheckoutRequestDto request)
{
    if (!ModelState.IsValid)
    {
        return BadRequest(ModelState);
    }

    var userId = GetUserIdFromClaims();

    // Load the active cart for this user with items and inventory details
    var cart = await _db.Carts
        .Include(s => s.User)
        .Include(c => c.Items)
            .ThenInclude(ci => ci.InventoryItem)
        .SingleOrDefaultAsync(c => c.UserId == userId && c.isActive);

    if (cart == null)
    {
        return BadRequest("No active cart found.");
    }

    if (cart.Items == null || cart.Items.Count == 0)
    {
        return BadRequest("Cart is empty.");
    }

    // Make sure none of the items are already sold
    var soldItems = cart.Items
        .Where(ci => ci.InventoryItem.IsSold)
        .ToList();

    if (soldItems.Any())
    {
        return BadRequest("One or more items in the cart are no longer available.");
    }

    // Each item is one-of-one, so subtotal is just sum of prices
    var subtotal = cart.Items.Sum(ci => ci.InventoryItem.Price);

    // 🔹 Shipping cost based on ShippingSpeed
    decimal shippingCost;
    string normalizedSpeed = request.ShippingSpeed.Trim();

    switch (normalizedSpeed.ToLowerInvariant())
    {
        case "overnight":
            shippingCost = 29m;
            normalizedSpeed = "Overnight";
            break;

        case "3-day":
        case "3 day":
        case "3day":
            shippingCost = 19m;
            normalizedSpeed = "3-Day";
            break;

        case "ground":
            shippingCost = 0m;
            normalizedSpeed = "Ground";
            break;

        default:
            return BadRequest("Invalid shipping speed. Must be 'Overnight', '3-Day', or 'Ground'.");
    }

    var tax = Math.Round(subtotal * TAX_RATE, 2, MidpointRounding.AwayFromZero);
    var total = subtotal + tax + shippingCost;

    var sale = new Sale
    {
        UserId = userId,
        SaleDateTime = DateTime.UtcNow,
        Subtotal = subtotal,
        Tax = tax,
        ShippingCost = shippingCost,
        Total = total,
        ShippingSpeed = normalizedSpeed,
        Street1 = request.Street1,
        Street2 = request.Street2,
        City = request.City,
        State = request.State,
        Zip = request.Zip,
        CardLast4 = request.CardLast4
    };

    _db.Sales.Add(sale);
    await _db.SaveChangesAsync(); // get SaleId

    // Create SaleItems (one per inventory item) and mark inventory as sold
    foreach (var ci in cart.Items)
    {
        var saleItem = new SaleItem
        {
            SaleId = sale.SaleId,
            ItemId = ci.ItemId,
            Quantity = 1, // always 1
            UnitPrice = ci.InventoryItem.Price
        };

        _db.SaleItems.Add(saleItem);

        // mark inventory item as sold
        ci.InventoryItem.IsSold = true;
    }

    // Deactivate cart
    cart.isActive = false;

    await _db.SaveChangesAsync();

    // Update for DTO 
    var completedSale = await _db.Sales
        .Include(s => s.User)  
        .Include(s => s.Items)
            .ThenInclude(si => si.InventoryItem)
        .SingleAsync(s => s.SaleId == sale.SaleId);

    var dto = MapToDetailDto(completedSale);
    return Ok(dto);
}

    // Helpers

    private int GetUserIdFromClaims()
    {
        // JWT created in AuthController: sub = UserId
        var sub = User.FindFirstValue(JwtRegisteredClaimNames.Sub)
                  ?? User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(sub))
        {
            throw new InvalidOperationException("No user id claim found in token.");
        }

        if (!int.TryParse(sub, out var userId))
        {
            throw new InvalidOperationException("Invalid user id claim.");
        }

        return userId;
    }

    private bool IsAdminFromClaims()
    {
        if (User.IsInRole("Admin"))
            return true;

        var isAdminClaim = User.FindFirstValue("isAdmin");
        return string.Equals(isAdminClaim, "true", StringComparison.OrdinalIgnoreCase);
    }

    private static OrderDetailDto MapToDetailDto(Sale sale)
    {
        var dto = new OrderDetailDto
        {
            SaleId = sale.SaleId,
            SaleDateTime = sale.SaleDateTime,
            Subtotal = sale.Subtotal,
            Tax = sale.Tax,
            ShippingCost = sale.ShippingCost,
            Total = sale.Total,
            ShippingSpeed = sale.ShippingSpeed,
            Street1 = sale.Street1,
            Street2 = sale.Street2,
            City = sale.City,
            State = sale.State,
            Zip = sale.Zip,
            CardLast4 = sale.CardLast4,
            
            UserId = sale.UserId,
            CustomerName = $"{sale.User.FirstName} {sale.User.LastName}",
            CustomerEmail = sale.User.Email
        };

        foreach (var si in sale.Items)
        {
            var item = si.InventoryItem;

            dto.Items.Add(new OrderItemDto
            {
                ItemId = si.ItemId,
                Name = item.Name,
                Price = si.UnitPrice,
                PrimaryPhotoUrl = item.PrimaryPhotoUrl
            });
        }

        return dto;
    }
}
