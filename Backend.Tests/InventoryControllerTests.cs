using System.Linq;
using System.Threading.Tasks;
using Backend.Controllers;
using Backend.DTOs;
using Backend.Tests;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace Backend.Tests.Controllers;

public class InventoryControllerTests
{
    // ---------- GET /api/inventory ----------

    [Fact]
    public async Task GetAll_ReturnsOk_WithAllItems()
    {
        // Arrange
        var ctx = TestDbContextFactory.CreateWithSeedData();
        var controller = new InventoryController(ctx);

        // Act
        var result = await controller.GetAll();

        // Assert
        var ok = Assert.IsType<OkObjectResult>(result);
        var items = Assert.IsAssignableFrom<System.Collections.IEnumerable>(ok.Value);

        var count = items.Cast<object>().Count();
        Assert.Equal(2, count);
    }

    // ---------- GET /api/inventory/{id} ----------

    [Fact]
    public async Task GetById_ExistingItem_ReturnsOk()
    {
        var ctx = TestDbContextFactory.CreateWithSeedData();
        var controller = new InventoryController(ctx);

        var result = await controller.GetById(1);

        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.NotNull(ok.Value);
    }

    [Fact]
    public async Task GetById_UnknownItem_ReturnsNotFound()
    {
        var ctx = TestDbContextFactory.CreateWithSeedData();
        var controller = new InventoryController(ctx);

        var result = await controller.GetById(999);

        Assert.IsType<NotFoundResult>(result);
    }

    // ---------- POST /api/inventory ----------

    [Fact]
    public async Task Create_ValidDto_InsertsItem_AndReturnsCreated()
    {
        var ctx = TestDbContextFactory.CreateEmpty();
        var controller = new InventoryController(ctx);

        var dto = new InventoryItemWriteDto
        {
            Name = "New Item",
            Description = "New Desc",
            Price = 99.99m,
            PrimaryPhotoUrl = "http://example.com/new",
            Category = "NewCat",
            IsSold = false
        };

        var result = await controller.Create(dto);

        var created = Assert.IsType<CreatedAtActionResult>(result);
        Assert.Equal("GetById", created.ActionName);

        var all = ctx.InventoryItems.ToList();
        Assert.Single(all);

        var item = all.Single();
        Assert.Equal("New Item", item.Name);
        Assert.Equal(99.99m, item.Price);
        Assert.Equal("NewCat", item.Category);
        Assert.False(item.IsSold);
    }

    // ---------- PUT /api/inventory/{id} ----------

    [Fact]
    public async Task Update_ExistingItem_UpdatesFields_AndReturnsOk()
    {
        var ctx = TestDbContextFactory.CreateWithSeedData();
        var controller = new InventoryController(ctx);

        var dto = new InventoryItemWriteDto
        {
            Name = "Updated Name",
            Description = "Updated Desc",
            Price = 123.45m,
            PrimaryPhotoUrl = "http://example.com/updated",
            Category = "UpdatedCat",
            IsSold = true
        };

        var result = await controller.Update(1, dto);

        var ok = Assert.IsType<OkObjectResult>(result);

        var item = ctx.InventoryItems.Single(i => i.ItemId == 1);
        Assert.Equal("Updated Name", item.Name);
        Assert.Equal("Updated Desc", item.Description);
        Assert.Equal(123.45m, item.Price);
        Assert.Equal("UpdatedCat", item.Category);
        Assert.True(item.IsSold);
    }

    [Fact]
    public async Task Update_UnknownItem_ReturnsNotFound()
    {
        var ctx = TestDbContextFactory.CreateWithSeedData();
        var controller = new InventoryController(ctx);

        var dto = new InventoryItemWriteDto
        {
            Name = "DoesNotMatter",
            Description = "x",
            Price = 1m,
            PrimaryPhotoUrl = "http://example.com/x",
            Category = "X",
            IsSold = false
        };

        var result = await controller.Update(999, dto);

        Assert.IsType<NotFoundResult>(result);
    }

    // ---------- DELETE /api/inventory/{id} ----------

    [Fact]
    public async Task Delete_ExistingItem_RemovesFromDatabase_AndReturnsNoContent()
    {
        var ctx = TestDbContextFactory.CreateWithSeedData();
        var controller = new InventoryController(ctx);

        Assert.True(ctx.InventoryItems.Any(i => i.ItemId == 1));

        var result = await controller.Delete(1);

        Assert.IsType<NoContentResult>(result);
        Assert.False(ctx.InventoryItems.Any(i => i.ItemId == 1));
    }

    [Fact]
    public async Task Delete_UnknownItem_ReturnsNotFound()
    {
        var ctx = TestDbContextFactory.CreateWithSeedData();
        var controller = new InventoryController(ctx);

        var result = await controller.Delete(999);

        Assert.IsType<NotFoundResult>(result);
    }
}
