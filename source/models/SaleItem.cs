  public class SaleItem
    {
        public int SaleId { get; set; }
        public int ItemId { get; set; }

        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }

        public Sale Sale { get; set; } = null!;
        public InventoryItem InventoryItem { get; set; } = null!;
    }
