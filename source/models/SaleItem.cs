using System.Collections.Generic;

namespace Backend.Models;

public class SaleItem
{
    public int SaleId { get; set; }
    public int ItemId { get; set; }

    public decimal UnitPrice { get; set; }

    public Sale Sale { get; set; } = null!;
    public InventoryItem InventoryItem { get; set; } = null!;
}
