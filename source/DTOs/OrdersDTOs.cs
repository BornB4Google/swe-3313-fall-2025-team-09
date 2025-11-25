using System;
usingusing System;
2
using System.Collections.Generic;
 |  | 
3
 
4
 
using System.ComponentModel.DataAnnotations;
5
 
​
6
 
namespace Backend.DTOs;
7
 
​
8
 
public class OrderItemDto
9
 
{
10
 
    public int ItemId { get; set; }
11
 
    public string Name { get; set; } = string.Empty;
12
 
    public decimal Price { get; set; }
13
 
    public string PrimaryPhotoUrl { get; set; } = string.Empty;
14
 
}
15
 
​
16
 
public class OrderSummaryDto
17
 
{
18
 
    public int SaleId { get; set; }
19
 
    public DateTime SaleDateTime { get; set; }
20
 
    public decimal Total { get; set; }
21
 
    public int ItemCount { get; set; }
22
 
}
23
 
​
24
 
public class OrderDetailDto
25
 
{
26
 
    public int SaleId { get; set; }
27
 
    public DateTime SaleDateTime { get; set; }
28
 
    public decimal Subtotal { get; set; }
29
 
    public decimal Tax { get; set; }
30
 
    public decimal ShippingCost { get; set; }
31
 
    public decimal Total { get; set; }
32
 
​
33
 
    public string ShippingSpeed { get; set; } = string.Empty;
34
 
    public string Street1 { get; set; } = string.Empty;
35
 
    public string? Street2 { get; set; }
36
 
    public string City { get; set; } = string.Empty;
37
 
    public string State { get; set; } = string.Empty;
38
 
    public string Zip { get; set; } = string.Empty;
39
 
    public string CardLast4 { get; set; } = string.Empty;
40
 
​
41
 
    public List<OrderItemDto> Items { get; set; } = new();
42
 
}
43
 
​
44
 
public class CheckoutRequestDto
45
 
{
46
 
    [Required]
47
 
    public string ShippingSpeed { get; set; } = string.Empty;
48
 
​
49
 
    [Required]
50
 
    public string Street1 { get; set; } = string.Empty;
51
 
    public string? Street2 { get; set; } System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Backend.DTOs;

public class OrderItemDto
{
    public int ItemId { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string PrimaryPhotoUrl { get; set; } = string.Empty;
}

public class OrderSummaryDto
{
    public int SaleId { get; set; }
    public DateTime SaleDateTime { get; set; }
    public decimal Total { get; set; }
    public int ItemCount { get; set; }
}

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

    public List<OrderItemDto> Items { get; set; } = new();
}

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