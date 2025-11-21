using Backend.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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

    // GET /api/reports/sales?startDate={date}&endDate={date}
    // Generates a sales report, optionally filtered by date range
    [HttpGet("sales")]
    public async Task<IActionResult> GetSalesReport(
        [FromQuery] DateTime? startDate,
        [FromQuery] DateTime? endDate)
    {
        // Andrew To Do: sales report logic with optional date filtering
        return StatusCode(501, "Not implemented yet");
    }

    // GET /api/reports/inventory
    // Gets inventory status report
    [HttpGet("inventory")]
    public async Task<IActionResult> GetInventoryReport()
    {
        // Andrew To Do: inventory status logic 
        
        return StatusCode(501, "Not implemented yet");
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