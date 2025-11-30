using Backend.Data;
using Backend.DTOs;
using Backend.Models;

using FuzzySharp;

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
    [Authorize(Roles = "User, Admin")]
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
    [Authorize(Roles = "User, Admin")]
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
    // Add an inventory item
    [HttpPost]
    [Authorize(Roles = "User, Admin")]
    public async Task<IActionResult> Create([FromBody] InventoryItemWriteDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var entity = new InventoryItem
        {
            Name = dto.Name,
            Description = dto.Description,
            Price = dto.Price,
            PrimaryPhotoUrl = dto.PrimaryPhotoUrl,
            Category = dto.Category,
            IsSold = dto.IsSold
        };

        _db.InventoryItems.Add(entity);
        await _db.SaveChangesAsync();

        var result = new
        {
            entity.ItemId,
            entity.Name,
            entity.Description,
            entity.Price,
            entity.PrimaryPhotoUrl,
            entity.Category,
            entity.IsSold,
            Images = Array.Empty<object>()
        };

        return CreatedAtAction(nameof(GetById), new { id = entity.ItemId }, result);
    }

    // PUT /api/inventory/{id} (Admin)
    // Update an inventory item
    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(int id, [FromBody] InventoryItemWriteDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var entity = await _db.InventoryItems
            .Include(i => i.Images)
            .FirstOrDefaultAsync(i => i.ItemId == id);

        if (entity is null)
            return NotFound();

        entity.Name = dto.Name;
        entity.Description = dto.Description;
        entity.Price = dto.Price;
        entity.PrimaryPhotoUrl = dto.PrimaryPhotoUrl;
        entity.Category = dto.Category;
        entity.IsSold = dto.IsSold;

        await _db.SaveChangesAsync();

        var result = new
        {
            entity.ItemId,
            entity.Name,
            entity.Description,
            entity.Price,
            entity.PrimaryPhotoUrl,
            entity.Category,
            entity.IsSold,
            Images = entity.Images
                .OrderBy(img => img.DisplayOrder)
                .Select(img => new
                {
                    img.ImageId,
                    img.ImageUrl,
                    img.DisplayOrder
                })
                .ToList()
        };

        return Ok(result);
    }

    // DELETE /api/inventory/{id} (Admin)
    // Delete an inventory item
    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var entity = await _db.InventoryItems
            .Include(i => i.Images)
            .FirstOrDefaultAsync(i => i.ItemId == id);

        if (entity is null)
            return NotFound();

        _db.InventoryItems.Remove(entity);
        await _db.SaveChangesAsync();

        return NoContent();
    }

    // GET /api/inventory/search?q=searchterm
    [HttpGet("search")]
    [Authorize(Roles = "User, Admin")]
    public async Task<IActionResult> Search([FromQuery] string q)
    {
        if (string.IsNullOrWhiteSpace(q))
            return BadRequest("Search query required");

        var items = await _db.InventoryItems
            .Include(i => i.Images)
            .Where(i => !i.IsSold)
            .ToListAsync();

        var scoredItems = items.Select(item =>
            {
                var nameScore = Fuzz.WeightedRatio(q, item.Name);
                var categoryScore = Fuzz.WeightedRatio(q, item.Category);
                var descScore = Fuzz.PartialRatio(q, item.Description);  // partial for long text

                var bestScore = Math.Max(nameScore, Math.Max(categoryScore, descScore));

                return new { Item = item, Score = bestScore };
            })
            .Where(x => x.Score >= 50)  // cutoff
            .OrderByDescending(x => x.Score)
            .Select(x => new
            {
                x.Item.ItemId,
                x.Item.Name,
                x.Item.Description,
                x.Item.Price,
                x.Item.PrimaryPhotoUrl,
                x.Item.Category,
                x.Item.IsSold,
                MatchScore = x.Score,
                Images = x.Item.Images
                    .OrderBy(img => img.DisplayOrder)
                    .Select(img => new
                    {
                        img.ImageId,
                        img.ImageUrl,
                        img.DisplayOrder
                    })
                    .ToList()
            })
            .ToList();

        return Ok(scoredItems);
    }
}
