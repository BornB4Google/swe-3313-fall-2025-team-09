    public class ItemImage
    {
        public int ImageId { get; set; }
        public int ItemId { get; set; }
        public string ImageUrl { get; set; } = "";
        public int DisplayOrder { get; set; }

        public InventoryItem InventoryItem { get; set; } = null!;
    }
