using System.ComponentModel.DataAnnotations;

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

public class InventoryItemWriteDto
{
    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [MaxLength(2000)]
    public string Description { get; set; } = string.Empty;

    [Range(0, double.MaxValue)]
    public decimal Price { get; set; }

    [Url]
    public string? PrimaryPhotoUrl { get; set; }

    [Required]
    [MaxLength(100)]
    public string Category { get; set; } = string.Empty;

    public bool IsSold { get; set; } = false;
}


