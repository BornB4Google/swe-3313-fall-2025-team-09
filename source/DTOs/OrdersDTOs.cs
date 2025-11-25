using System;
using System.Collections.Generic;

namespace Backend.DTOs;

// One item within an order
public class OrderItemDto
{
    public int ItemId { get; set; }      
    public string Name { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;

    public decimal UnitPrice { get; set; }

 
    public decimal Subtotal => UnitPrice;
}

// An order placed by a user
public class OrderDto
{
    public int OrderId { get; set; }
    public int UserId { get; set; }

    public DateTime CreatedAt { get; set; }
    public string Status { get; set; } = string.Empty;   // "Pending", "Completed", etc.

    public List<OrderItemDto> Items { get; set; } = new();

    public decimal Total { get; set; }
}

// Request body for POST /api/orders/checkout
public class CheckoutRequest
{
    public string Notes { get; set; } = string.Empty;
}