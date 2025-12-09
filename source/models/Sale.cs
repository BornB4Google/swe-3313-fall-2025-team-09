using System;
using System.Collections.Generic;

namespace Backend.Models;

public class Sale
{
    public int SaleId { get; set; }
    public int UserId { get; set; }

    public string CheckoutName { get; set; }

    public DateTime SaleDateTime { get; set; }
    public decimal Subtotal { get; set; }
    public decimal Tax { get; set; }
    public decimal ShippingCost { get; set; }
    public decimal Total { get; set; }
    public string ShippingSpeed { get; set; } = "";
    public string Street1 { get; set; } = "";
    public string? Street2 { get; set; }
    public string City { get; set; } = "";
    public string State { get; set; } = "";
    public string Zip { get; set; } = "";
    public string CardLast4 { get; set; } = "";

    public User User { get; set; } = null!;
    public List<SaleItem> Items { get; set; } = new();
}
