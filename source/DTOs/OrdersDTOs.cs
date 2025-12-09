using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Backend.DTOs;

// One item within an order
public class OrderItemDto
{
    public int ItemId { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string PrimaryPhotoUrl { get; set; } = string.Empty;
}

// Summary info for listing orders
public class OrderSummaryDto
{
    public int SaleId { get; set; }
    public DateTime SaleDateTime { get; set; }
    public decimal Total { get; set; }
    public int ItemCount { get; set; }

    public int UserId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public string CustomerEmail { get; set; } = string.Empty;
}

// Full details for a single order
public class OrderDetailDto
{
    public int SaleId { get; set; }
    public DateTime SaleDateTime { get; set; }
    public decimal Subtotal { get; set; }
    public decimal Tax { get; set; }
    public decimal ShippingCost { get; set; }
    public decimal Total { get; set; }

    public string ShippingSpeed { get; set; } = string.Empty;
    public string Street1 { get; set; } = string.Empty;
    public string? Street2 { get; set; }
    public string City { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string Zip { get; set; } = string.Empty;
    public string CardLast4 { get; set; } = string.Empty;

    // Customer Info

    public int UserId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public string CustomerEmail { get; set; } = string.Empty;

    public List<OrderItemDto> Items { get; set; } = new();
}

// Request body for POST /api/orders/checkout
public class CheckoutRequestDto
{
    [Required]
    [StringLength(50, MinimumLength = 1, ErrorMessage = "First name is required.")]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [StringLength(50, MinimumLength = 1, ErrorMessage = "Last name is required.")]
    public string LastName { get; set; } = string.Empty;

    [Required]
    [StringLength(20)]
    public string ShippingSpeed { get; set; } = string.Empty;

    [Required]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "Street1 must be at least 3 characters.")]
    public string Street1 { get; set; } = string.Empty;

    [StringLength(100)] public string? Street2 { get; set; }

    [Required]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "City must be at least 2 characters.")]
    public string City { get; set; } = string.Empty;

    [Required]
    [RegularExpression(@"^[A-Za-z]{2}$",
        ErrorMessage = "State must be a 2-letter code.")]
    public string State { get; set; } = string.Empty;

    [Required]
    [RegularExpression(@"^\d{5}(-\d{4})?$",
        ErrorMessage = "Zip must be 5 digits or ZIP+4 (12345 or 12345-6789).")]
    public string Zip { get; set; } = string.Empty;

    [Required]
    [RegularExpression(@"^\d{4}$",
        ErrorMessage = "CardLast4 must be exactly 4 digits.")]
    public string CardLast4 { get; set; } = string.Empty;

    [Required]
    [RegularExpression(@"^(0[1-9]|1[0-2])\/\d{4}$",
        ErrorMessage = "Expiration must be in MM/YYYY format.")]
    public string Expiration { get; set; } = string.Empty;

}
