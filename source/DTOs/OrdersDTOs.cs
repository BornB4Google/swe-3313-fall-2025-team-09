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
    public string ShippingSpeed { get; set; } = string.Empty;

    [Required]
    public string Street1 { get; set; } = string.Empty;

    public string? Street2 { get; set; }

    [Required]
    public string City { get; set; } = string.Empty;

    [Required]
    [MinLength(2)]
    public string State { get; set; } = string.Empty;

    [Required]
    [MinLength(5)]
    public string Zip { get; set; } = string.Empty;

    [Required]
    [MinLength(4), MaxLength(4)]
    public string CardLast4 { get; set; } = string.Empty;
}
