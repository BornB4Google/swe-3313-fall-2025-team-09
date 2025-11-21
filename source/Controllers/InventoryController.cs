using Backend.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class InventoryController : ControllerBase
{
    private readonly StorefrontDbContext _db;

    public InventoryController(StorefrontDbContext db)
    {
        _db = db;
    }
    
    // Get /api/inventory
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll()
    {
        var items = await _db.InventoryItems
            .Include(i => i.Images)
            .OrderBy(i => i.ItemId)
            .Select(i => new
            {
                i.ItemId,
                i.Name,
                i.Description,
                i.Price,
                i.PrimaryPhotoUrl,
                i.Category,
                i.IsSold,
                Images = i.Images
                    .OrderBy(img => img.DisplayOrder)
                    .Select(img => new
                    {
                        img.ImageId,
                        img.ImageUrl,
                        img.DisplayOrder
                    })
                    .ToList()
            })
            .ToListAsync();

        return Ok(items);
    }
    
    // Get /api/inventory/{id}
    [HttpGet("{id:int}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetById(int id)
    {
        var item = await _db.InventoryItems
            .Include(i => i.Images)
            .Where(i => i.ItemId == id)
            .Select(i => new
            {
                i.ItemId,
                i.Name,
                i.Description,
                i.Price,
                i.PrimaryPhotoUrl,
                i.Category,
                i.IsSold,
                Images = i.Images
                    .OrderBy(img => img.DisplayOrder)
                    .Select(img => new
                    {
                        img.ImageId,
                        img.ImageUrl,
                        img.DisplayOrder
                    })
                    .ToList()
            })
            .FirstOrDefaultAsync();

        if (item is null)
            return NotFound();

        return Ok(item);
    }
    
    // POST /api/inventory  (Admin)
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public IActionResult Create()
    {
        // ANDREW TO DO : implement create inventory item business logic
        return StatusCode(501, "Not implemented yet");
    }

    // PUT /api/inventory/{id} (Admin)
    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin")]
    public IActionResult Update(int id)
    {
        // Andrew To Do: implement update inventory item logic
        return StatusCode(501, "Not implemented yet");
    }

    // DELETE /api/inventory/{id} (Admin)
    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    public IActionResult Delete(int id)
    {
        // Andrew To Do: implement delete item logic
        return StatusCode(501, "Not implemented yet");
    }
}