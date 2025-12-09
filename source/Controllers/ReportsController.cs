using Backend.Data;
using Backend.Models;
using Backend.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Authorization;
using System.Text;


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

    // GET /api/reports/sales/csv
    // Exports all sales ever made as CSV
    [HttpGet("sales/csv")]
    public async Task<IActionResult> GetAllSalesCsv()
    {
        var sales = await _db.Sales
            .OrderBy(s => s.SaleDateTime)
            .Select(s => new
            {
                s.SaleId,
                s.UserId,
                s.SaleDateTime,
                s.Subtotal,
                s.Tax,
                s.ShippingCost,
                s.Total,
                s.ShippingSpeed,
                s.Street1,
                s.Street2,
                s.City,
                s.State,
                s.Zip,
                s.CardLast4
            })
            .ToListAsync();

        var sb = new StringBuilder();

        // Header row
        sb.AppendLine("SaleId,UserId,SaleDateTime,Subtotal,Tax,ShippingCost,Total,ShippingSpeed,Street1,Street2,City,State,Zip,CardLast4");

        foreach (var s in sales)
        {
            sb.AppendLine(
                $"{s.SaleId}," +
                $"{s.UserId}," +
                $"{s.SaleDateTime:yyyy-MM-dd HH:mm:ss}," +
                $"{s.Subtotal:F2}," +
                $"{s.Tax:F2}," +
                $"{s.ShippingCost:F2}," +
                $"{s.Total:F2}," +
                $"{s.ShippingSpeed}," +
                $"{s.Street1}," +
                $"{s.Street2}," +
                $"{s.City}," +
                $"{s.State}," +
                $"{s.Zip}," +
                $"{s.CardLast4}");
        }

        var bytes = Encoding.UTF8.GetBytes(sb.ToString());
        var fileName = $"sales-all.csv";

        return File(bytes, "text/csv", fileName);
    }

    // GET /api/reports/revenue
    // Returns overall revenue breakdown, with the option of a specific day
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

    // GET /api/reports/sales/weekly
    // Weekly data for the week starting at startDate
    [HttpGet("sales/weekly")]
    public async Task<IActionResult> GetWeeklySalesReport(
        [FromQuery] DateTime? startDate)
    {

        var today = DateTime.UtcNow.Date;
        var weekStart = (startDate ?? today.AddDays(-7)).Date;
        var weekEnd = weekStart.AddDays(7);

        var salesQuery = _db.Sales
            .Where(s => s.SaleDateTime >= weekStart && s.SaleDateTime < weekEnd);

        var stats = await salesQuery
            .GroupBy(_ => 1)
            .Select(g => new
            {
                OrderCount = g.Count(),
                Total = g.Sum(s => s.Total),
                Subtotal = g.Sum(s => s.Subtotal),
                Tax = g.Sum(s => s.Tax),
                Shipping = g.Sum(s => s.ShippingCost)
            })
            .FirstOrDefaultAsync();

        stats ??= new { OrderCount = 0, Total = 0m, Subtotal = 0m, Tax = 0m, Shipping = 0m };

        var weeklyDataPoint = new
        {
            WeekStart = weekStart,
            WeekEnd = weekEnd.AddDays(-1),
            OrderCount = stats.OrderCount,
            Total = stats.Total,
            Subtotal = stats.Subtotal,
            Tax = stats.Tax,
            Shipping = stats.Shipping
        };

        var summary = new
        {
            TotalOrders = weeklyDataPoint.OrderCount,
            TotalRevenue = weeklyDataPoint.Total,
            TotalSubtotal = weeklyDataPoint.Subtotal,
            TotalTax = weeklyDataPoint.Tax,
            TotalShipping = weeklyDataPoint.Shipping
        };

        return Ok(new
        {
            Summary = summary,
            WeeklyDataPoints = new[] { weeklyDataPoint }
        });
    }
    // GET /api/reports/sales/weekly/csv
    // Exports all sales the week following startDate as CSV
    [HttpGet("sales/weekly/csv")]
    public async Task<IActionResult> GetWeeklySalesCsv(
        [FromQuery] DateTime? startDate)
    {
        // If the client passed an invalid date, give an error
        if (!ModelState.IsValid)
        {
            return BadRequest("startDate must be a valid date. Example: startDate=2025-11-01");
        }

        // Default to the previous 7-day window if no startDate is provided
        var today = DateTime.UtcNow.Date;
        var weekStart = (startDate ?? today.AddDays(-7)).Date;
        var weekEnd = weekStart.AddDays(7);

        var sales = await _db.Sales
            .Where(s => s.SaleDateTime >= weekStart && s.SaleDateTime < weekEnd)
            .OrderBy(s => s.SaleDateTime)
            .Select(s => new
            {
                s.SaleId,
                s.UserId,
                s.SaleDateTime,
                s.Subtotal,
                s.Tax,
                s.ShippingCost,
                s.Total,
                s.ShippingSpeed,
                s.Street1,
                s.Street2,
                s.City,
                s.State,
                s.Zip,
                s.CardLast4
            })
            .ToListAsync();

        var sb = new StringBuilder();

        // Header row
        sb.AppendLine("SaleId,UserId,SaleDateTime,Subtotal,Tax,ShippingCost,Total,ShippingSpeed,Street1,Street2,City,State,Zip,CardLast4");

        foreach (var s in sales)
        {
            sb.AppendLine(
                $"{s.SaleId}," +
                $"{s.UserId}," +
                $"{s.SaleDateTime:yyyy-MM-dd HH:mm:ss}," +
                $"{s.Subtotal:F2}," +
                $"{s.Tax:F2}," +
                $"{s.ShippingCost:F2}," +
                $"{s.Total:F2}," +
                $"{s.ShippingSpeed}," +
                $"{s.Street1}," +
                $"{s.Street2}," +
                $"{s.City}," +
                $"{s.State}," +
                $"{s.Zip}," +
                $"{s.CardLast4}");
        }

        var bytes = Encoding.UTF8.GetBytes(sb.ToString());
        var fileName = $"sales-week-starting-{weekStart:yyyyMMdd}.csv";

        return File(bytes, "text/csv", fileName);
    }

    // GET /api/reports/sales/monthly
    // Monthly revenue data points for a given year (for bar graph),
    // with optional filter for specific month (monthly sales report)
    [HttpGet("sales/monthly")]
    public async Task<IActionResult> GetMonthlySalesReport(
        [FromQuery] int? year,
        [FromQuery] int? month)
    {
        int targetYear = year ?? DateTime.UtcNow.Year;

        // Whole year
        var start = new DateTime(targetYear, 1, 1);
        var end = start.AddYears(1);

        var salesQuery = _db.Sales
            .Where(s => s.SaleDateTime >= start && s.SaleDateTime < end);

        // Specific month
        if (month.HasValue && month.Value >= 1 && month.Value <= 12)
        {
            salesQuery = salesQuery.Where(s => s.SaleDateTime.Month == month.Value);
        }

        var monthlyDataPoints = await salesQuery
            .GroupBy(s => s.SaleDateTime.Month)
            .Select(g => new
            {
                Month = g.Key,
                MonthStart = new DateTime(targetYear, g.Key, 1),
                MonthEnd = new DateTime(targetYear, g.Key, 1).AddMonths(1).AddDays(-1),
                OrderCount = g.Count(),
                Total = g.Sum(s => s.Total),
                Subtotal = g.Sum(s => s.Subtotal),
                Tax = g.Sum(s => s.Tax),
                Shipping = g.Sum(s => s.ShippingCost)
            })
            .OrderBy(x => x.Month)
            .ToListAsync();

        var summary = new
        {
            TotalOrders = monthlyDataPoints.Sum(p => p.OrderCount),
            TotalRevenue = monthlyDataPoints.Sum(p => p.Total),
            TotalSubtotal = monthlyDataPoints.Sum(p => p.Subtotal),
            TotalTax = monthlyDataPoints.Sum(p => p.Tax),
            TotalShipping = monthlyDataPoints.Sum(p => p.Shipping)
        };

        return Ok(new
        {
            Summary = summary,
            MonthlyDataPoints = monthlyDataPoints
        });
    }

    // GET /api/reports/sales/monthly/csv
    // Exports all sales for a specific month as CSV
    [HttpGet("sales/monthly/csv")]
    public async Task<IActionResult> GetMonthlySalesCsv(
        [FromQuery] int? year,
        [FromQuery] int? month)
    {
        var now = DateTime.UtcNow;
        int targetYear;
        int targetMonth;

        if (!year.HasValue && !month.HasValue)
        {
            // Default to previous calendar month
            var prevMonthDate = new DateTime(now.Year, now.Month, 1).AddMonths(-1);
            targetYear = prevMonthDate.Year;
            targetMonth = prevMonthDate.Month;
        }
        else
        {
            if (!year.HasValue || !month.HasValue || year <= 0 || month < 1 || month > 12)
            {
                return BadRequest("Year and month must be valid. Example: year=2025&month=11");
            }

            targetYear = year.Value;
            targetMonth = month.Value;
        }

        var monthStart = new DateTime(targetYear, targetMonth, 1);
        var monthEnd = monthStart.AddMonths(1);

        var sales = await _db.Sales
            .Where(s => s.SaleDateTime >= monthStart && s.SaleDateTime < monthEnd)
            .OrderBy(s => s.SaleDateTime)
            .Select(s => new
            {
                s.SaleId,
                s.UserId,
                s.SaleDateTime,
                s.Subtotal,
                s.Tax,
                s.ShippingCost,
                s.Total,
                s.ShippingSpeed,
                s.Street1,
                s.Street2,
                s.City,
                s.State,
                s.Zip,
                s.CardLast4
            })
            .ToListAsync();

        var sb = new StringBuilder();

        // Header row
        sb.AppendLine("SaleId,UserId,SaleDateTime,Subtotal,Tax,ShippingCost,Total,ShippingSpeed,Street1,Street2,City,State,Zip,CardLast4");

        foreach (var s in sales)
        {
            sb.AppendLine(
                $"{s.SaleId}," +
                $"{s.UserId}," +
                $"{s.SaleDateTime:yyyy-MM-dd HH:mm:ss}," +
                $"{s.Subtotal:F2}," +
                $"{s.Tax:F2}," +
                $"{s.ShippingCost:F2}," +
                $"{s.Total:F2}," +
                $"{s.ShippingSpeed}," +
                $"{s.Street1}," +
                $"{s.Street2}," +
                $"{s.City}," +
                $"{s.State}," +
                $"{s.Zip}," +
                $"{s.CardLast4}");
        }

        var bytes = Encoding.UTF8.GetBytes(sb.ToString());
        var fileName = $"sales-{targetYear}-{targetMonth:00}.csv";

        return File(bytes, "text/csv", fileName);
    }


    // GET /api/reports/recent-sales
    // Returns details on the three most recent sales and connects to their receipt
    [HttpGet("recent-sales")]
    public async Task<IActionResult> GetRecentSales()
    {
        var recentItems = await _db.SaleItems
            .Include(si => si.Sale)
            .Include(si => si.InventoryItem)
            .OrderByDescending(si => si.Sale.SaleDateTime)
            .Take(10)
            .Select(si => new
            {
                ItemName = si.InventoryItem.Name,
                ItemDescription = si.InventoryItem.Description,
                DateSold = si.Sale.SaleDateTime,
                LineTotal = si.UnitPrice,

                // key for linking to the receipt
                SaleId = si.SaleId
            })
            .ToListAsync();

        return Ok(recentItems);
    }

    // GET /api/orders/search
    // Search receipts by saleId, customer email, and optional date range
    [HttpGet("search")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<IEnumerable<OrderSummaryDto>>> SearchOrders(
        [FromQuery] int? saleId,
        [FromQuery] string? customerEmail,
        [FromQuery] DateTime? startDate,
        [FromQuery] DateTime? endDate)
    {
        IQueryable<Sale> query = _db.Sales
            .AsNoTracking()
            .Include(s => s.User)
            .Include(s => s.Items);

        if (saleId.HasValue)
        {
            query = query.Where(s => s.SaleId == saleId.Value);
        }

        if (!string.IsNullOrWhiteSpace(customerEmail))
        {
            var lowered = customerEmail.ToLower();
            query = query.Where(s => s.User.Email.ToLower().Contains(lowered));
        }

        if (startDate.HasValue)
        {
            query = query.Where(s => s.SaleDateTime >= startDate.Value);
        }

        if (endDate.HasValue)
        {
            query = query.Where(s => s.SaleDateTime <= endDate.Value);
        }

        var sales = await query
            .OrderByDescending(s => s.SaleDateTime)
            .ToListAsync();

        var result = sales.Select(s => new OrderSummaryDto
        {
            SaleId = s.SaleId,
            SaleDateTime = s.SaleDateTime,
            Total = s.Total,
            ItemCount = s.Items.Count,
            UserId = s.UserId,
            CustomerName = $"{s.User.FirstName} {s.User.LastName}",
            CustomerEmail = s.User.Email
        }).ToList();

        return Ok(result);
    }

    // GET /api/reports/sold-items/search
    // Search sold items by name and/or itemId, and optional date range
    [HttpGet("sold-items/search")]
    public async Task<IActionResult> SearchSoldItems(
        [FromQuery] string? name,
        [FromQuery] int? itemId,
        [FromQuery] DateTime? startDate,
        [FromQuery] DateTime? endDate)
    {
        var query = _db.SaleItems
            .Include(si => si.Sale)
            .Include(si => si.InventoryItem)
            .AsQueryable();

        // Filter by itemId
        if (itemId.HasValue)
        {
            query = query.Where(si => si.ItemId == itemId.Value);
        }

        // Filter by item name (partial)
        if (!string.IsNullOrWhiteSpace(name))
        {
            var lowered = name.ToLower();
            query = query.Where(si =>
                si.InventoryItem.Name.ToLower().Contains(lowered));
        }

        // Filter by date range
        if (startDate.HasValue)
        {
            query = query.Where(si => si.Sale.SaleDateTime >= startDate.Value);
        }

        if (endDate.HasValue)
        {
            query = query.Where(si => si.Sale.SaleDateTime <= endDate.Value);
        }

        var results = await query
            .OrderByDescending(si => si.Sale.SaleDateTime)
            .Select(si => new
            {
                ItemId = si.ItemId,
                ItemName = si.InventoryItem.Name,
                ItemDescription = si.InventoryItem.Description,

                DateSold = si.Sale.SaleDateTime,
                LineTotal = si.UnitPrice,

                // Link to the receipt
                SaleId = si.SaleId
            })
            .ToListAsync();

        return Ok(results);
    }
}
