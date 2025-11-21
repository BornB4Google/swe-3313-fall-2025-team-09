namespace Backend.DTOs;

public class InventoryItemDto
{
    public int ItemId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string PrimaryPhotoUrl { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public bool IsSold { get; set; }
    public List<InventoryImageDto> Images { get; set; } = new();
}

public class InventoryImageDto
{
    public int ImageId { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public int DisplayOrder { get; set; }
}