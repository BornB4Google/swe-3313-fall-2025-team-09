using System;
using Backend.Data;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Tests;

public static class TestDbContextFactory
{
    /// <summary>
    /// Create a fresh in-memory context with no data.
    /// </summary>
    public static StorefrontDbContext CreateEmpty()
    {
        var options = new DbContextOptionsBuilder<StorefrontDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var ctx = new StorefrontDbContext(options);
        ctx.Database.EnsureCreated();
        return ctx;
    }

    /// <summary>
    /// Create a fresh in-memory context pre-seeded with a couple of InventoryItems.
    /// </summary>
    public static StorefrontDbContext CreateWithSeedData()
    {
        var ctx = CreateEmpty();

        ctx.InventoryItems.AddRange(
            new InventoryItem
            {
                ItemId = 1,
                Name = "Test Item 1",
                Description = "First test item",
                Price = 10m,
                PrimaryPhotoUrl = "http://example.com/1",
                Category = "CategoryA",
                IsSold = false
            },
            new InventoryItem
            {
                ItemId = 2,
                Name = "Test Item 2",
                Description = "Second test item",
                Price = 20m,
                PrimaryPhotoUrl = "http://example.com/2",
                Category = "CategoryB",
                IsSold = true
            }
        );

        ctx.SaveChanges();
        return ctx;
    }
}
}