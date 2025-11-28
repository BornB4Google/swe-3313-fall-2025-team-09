using System.Collections.Generic;

namespace Backend.DTOs;

public class AddToCartRequest
{
    public int ItemId { get; set; }
}

public class CartItemDto
{
    public int CartItemId { get; set; }

    public int ItemId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;

    public decimal UnitPrice { get; set; }

    public decimal Subtotal => UnitPrice;

    public string PrimaryPhotoUrl { get; set; } = string.Empty;
}

// Overall cart for the current user
public class CartDto
{
    public List<CartItemDto> Items { get; set; } = new();

    // Total is sum of all Subtotal values
    public decimal Total { get; set; }
}
