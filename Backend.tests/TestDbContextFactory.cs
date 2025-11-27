using System;
using Backend.Data;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Tests;

public static class TestDbContextFactory
{
    /// <summary>
    /// Base helper: create a context with a specific in-memory database name.
    /// </summary>
    public static StorefrontDbContext CreateContext(string dbName)
    {
        var options = new DbContextOptionsBuilder<StorefrontDbContext>()
            .UseInMemoryDatabase(databaseName: dbName)
            .EnableSensitiveDataLogging()
            .Options;

        var ctx = new StorefrontDbContext(options);
        ctx.Database.EnsureCreated();
        return ctx;
    }

    /// <summary>
    /// Create a fresh in-memory context with no preset data.
    /// </summary>
    public static StorefrontDbContext CreateEmpty()
        => CreateContext(Guid.NewGuid().ToString());

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
