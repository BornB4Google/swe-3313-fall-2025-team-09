using Backend.Data;
using Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Authorization;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class ReportsController : ControllerBase
{
    private readonly StorefrontDbContext _db;

    public ReportsController(StorefrontDbContext db)
    {
        _db = db;
    }

    // GET /api/reports/sales
    // Generates a sales report, optionally filtered by date range
    [HttpGet("sales")]
    public async Task<IActionResult> GetSalesReport(
        [FromQuery] DateTime? startDate,
        [FromQuery] DateTime? endDate)
    {
        var salesQuery = _db.Sales.AsQueryable();

        if (startDate.HasValue)
        {
            Expression<Func<Sale, bool>> startFilter =
                sale => sale.SaleDateTime >= startDate.Value;
            salesQuery = salesQuery.Where(startFilter);
        }

        if (endDate.HasValue)
        {
            Expression<Func<Sale, bool>> endFilter =
                sale => sale.SaleDateTime <= endDate.Value;
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

        return Ok(items);
    }

    // GET /api/reports/revenue
    // Returns revenue breakdown (overall and by day), with optional date range
    [HttpGet("revenue")]
    public async Task<IActionResult> GetRevenueReport(
        [FromQuery] DateTime? startDate,
        [FromQuery] DateTime? endDate)
    {
        var salesQuery = _db.Sales.AsQueryable();

        if (startDate.HasValue)
        {
            Expression<Func<Sale, bool>> startFilter =
                sale => sale.SaleDateTime >= startDate.Value;
            salesQuery = salesQuery.Where(startFilter);
        }

        if (endDate.HasValue)
        {
            Expression<Func<Sale, bool>> endFilter =
                sale => sale.SaleDateTime <= endDate.Value;
            salesQuery = salesQuery.Where(endFilter);
        }

        // Overall summary
        // Overall summary
        var summary = await salesQuery
                          .GroupBy(_ => 1)
                          .Select(g => new
                          {
                              OrderCount = g.Count(),
                              Subtotal = g.Sum(s => s.Subtotal),
                              Tax = g.Sum(s => s.Tax),
                              Shipping = g.Sum(s => s.ShippingCost),
                              TotalRevenue = g.Sum(s => s.Total)
                          })
                          .FirstOrDefaultAsync()
                      ?? new
                      {
                          OrderCount = 0,
                          Subtotal = 0m,
                          Tax = 0m,
                          Shipping = 0m,
                          TotalRevenue = 0m
                      };


        // Breakdown by day
        var byDay = await salesQuery
            .GroupBy(s => s.SaleDateTime.Date)
            .Select(g => new
            {
                Date = g.Key,
                OrderCount = g.Count(),
                Subtotal = g.Sum(s => s.Subtotal),
                Tax = g.Sum(s => s.Tax),
                Shipping = g.Sum(s => s.ShippingCost),
                Total = g.Sum(s => s.Total)
            })
            .OrderBy(x => x.Date)
            .ToListAsync();

        var report = new
        {
            Summary = summary,
            ByDay = byDay
        };


        return Ok(report);
    }
}
