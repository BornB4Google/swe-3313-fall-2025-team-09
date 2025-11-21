 public class InventoryItem
    {
        public int ItemId { get; set; }
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public decimal Price { get; set; }
        public string PrimaryPhotoUrl { get; set; } = "";
        public string Category { get; set; } = "";
        public bool IsSold { get; set; }

        public List<ItemImage> Images { get; set; } = new();
        
        public List<SaleItem> SaleItems { get; set; } = new();
        public List<CartItem> CartItems { get; set; } = new();
    }
