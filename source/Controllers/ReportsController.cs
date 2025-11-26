using Backend.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Backend.Models;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
// [Authorize(Roles = "Admin")] //commented for testing
public class ReportsController : ControllerBase
{
    private readonly StorefrontDbContext _db;

    public ReportsController(StorefrontDbContext db)
    {
        _db = db;
    }

    // GET /api/reports/sales?startDate={date}&endDate={date}
    // Generates a sales report, optionally filtered by date range
    [HttpGet("sales")]
    public async Task<IActionResult> GetSalesReport(
        [FromQuery] DateTime? startDate,
        [FromQuery] DateTime? endDate)
    {
        // Andrew To Do: sales report logic with optional date filtering
        var salesQuery = _db.Sales.AsQueryable();

        if (startDate.HasValue)
        {
            Expression<Func<Sale, bool>> startFilter = sale => sale.SaleDateTime >= startDate.Value;
            salesQuery = salesQuery.Where(startFilter);
        }

        if (endDate.HasValue)
        {
            Expression<Func<Sale, bool>> endFilter = sale => sale.SaleDateTime <= endDate.Value;
            salesQuery = salesQuery.Where(endFilter);
        }

        var items = await salesQuery
            .OrderBy(sale => sale.SaleDateTime)
            .Select(sale => new
            {
                sale.SaleId,
                sale.UserId,
                sale.SaleDateTime,
                sale.Subtotal,
                sale.Tax,
                sale.ShippingCost,
                sale.Total,
                sale.ShippingSpeed,
                sale.Street1,
                sale.Street2,
                sale.City,
                sale.State,
                sale.Zip,
                sale.CardLast4

            })
            .ToListAsync();

        //   return Ok();
        return Ok(items);



        // return StatusCode(501, "Not implemented yet");
    }

    // GET /api/reports/revenue
    // Returns revenue breakdown
    [HttpGet("revenue")]
    public async Task<IActionResult> GetRevenueReport()
    {
        // Andrew To Do- Revenue breakdown logic
        return StatusCode(501, "Not implemented yet");
    }
}
